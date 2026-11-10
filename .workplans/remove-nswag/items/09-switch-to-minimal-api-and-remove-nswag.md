Status: todo
Goal: Switch the WebApi project to Minimal API and remove NSwag in one atomic change, so the solution builds green at every commit boundary.
Files: `workshop/src/ClockOrDie.WebApi/Dtos.cs`, `workshop/src/ClockOrDie.WebApi/ApiResults.cs`, `workshop/src/ClockOrDie.WebApi/Endpoints.cs`, `workshop/src/ClockOrDie.WebApi/Program.cs`, `workshop/src/ClockOrDie.WebApi/Controllers/` (delete), `workshop/src/ClockOrDie.WebApi/Globals.cs` (delete), `workshop/src/ClockOrDie.WebApi/AppErrorExt.cs` (delete), `workshop/nswag.json` (delete), `workshop/api-definition/` (delete), `workshop/TODO/minimal-api-plan.md` (delete), `workshop/paket.dependencies`, `workshop/src/ClockOrDie.WebApi/paket.references`, `workshop/src/ClockOrDie.WebApi/Properties/launchSettings.json`
Steps:
1. Create `workshop/src/ClockOrDie.WebApi/Dtos.cs` with exactly:
   ```csharp
   namespace ClockOrDie.WebApi;

   public record ActivityDescription(string Description, string[]? Tags);

   public record Activity(long ActivityId, string ActivityLabel);

   public record ActivityTracking(long ActivityId, string ActivityLabel, double TimeTracked);

   public record TimeSheetCell(string Date, bool Validated, ActivityTracking[] Activities);

   public record TimeSheet(int WeekNumber, int WeekYear, TimeSheetCell[] TimeCells);

   public record ApiError(string Code, string Message);
   ```
2. Create `workshop/src/ClockOrDie.WebApi/ApiResults.cs` with exactly:
   ```csharp
   using ClockOrDie.Core;
   using Microsoft.FSharp.Core;

   namespace ClockOrDie.WebApi;

   public static class ApiResults
   {
       public static IResult ToHttpResult<T>(this FSharpResult<T, Domain.ApplicationError> result)
           where T : notnull
           => result.IsOk
               ? Results.Ok(result.ResultValue)
               : result.ErrorValue.ToHttpResult();

       public static IResult ToHttpResult<T>(
           this FSharpResult<T, Domain.ApplicationError> result,
           Func<T, IResult> onSuccess)
           where T : notnull
           => result.IsOk
               ? onSuccess(result.ResultValue)
               : result.ErrorValue.ToHttpResult();

       public static IResult ToHttpResult(this Domain.ApplicationError error)
       {
           switch (error.Tag)
           {
               case Domain.ApplicationError.Tags.BusinessErr:
                   var business = (Domain.ApplicationError.BusinessErr)error;
                   return Results.BadRequest(ToApiErrors("Business_Error", business.Item));

               case Domain.ApplicationError.Tags.TechnicalErr:
                   var technical = (Domain.ApplicationError.TechnicalErr)error;
                   return Results.Json(
                       ToApiErrors("Technical_Error", technical.Item),
                       statusCode: StatusCodes.Status500InternalServerError);

               default:
                   return Results.StatusCode(StatusCodes.Status501NotImplemented);
           }
       }

       private static ApiError[] ToApiErrors(string code, IEnumerable<string> messages)
           => messages.Select(message => new ApiError(code, message)).ToArray();
   }
   ```
3. Create `workshop/src/ClockOrDie.WebApi/Endpoints.cs` with exactly:
   ```csharp
   using ClockOrDie.Core;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.FSharp.Core;

   namespace ClockOrDie.WebApi;

   public static class Endpoints
   {
       public static IEndpointRouteBuilder MapClockOrDieEndpoints(this IEndpointRouteBuilder app)
       {
           app.MapPost("/activity-tracking/save",
                   ([FromBody] TimeSheetCell[] cells) => Results.StatusCode(StatusCodes.Status501NotImplemented))
               .WithTags("timesheets")
               .WithSummary("Save activities tracking, for later validation.")
               .Accepts<TimeSheetCell[]>("application/json")
               .Produces(StatusCodes.Status200OK)
               .Produces<ApiError[]>(StatusCodes.Status400BadRequest)
               .Produces<ApiError[]>(StatusCodes.Status500InternalServerError);

           app.MapGet("/timesheets/week/current",
                   () => Results.StatusCode(StatusCodes.Status501NotImplemented))
               .WithTags("timesheets")
               .WithSummary("Ask for the timesheet for the current week")
               .Produces<TimeSheet>(StatusCodes.Status200OK)
               .Produces<ApiError[]>(StatusCodes.Status500InternalServerError);

           app.MapGet("/timesheets/week/{week_number:int}/{week_year:int}",
                   ([FromRoute(Name = "week_number")] int weekNumber,
                    [FromRoute(Name = "week_year")] int weekYear) =>
                       Results.StatusCode(StatusCodes.Status501NotImplemented))
               .WithTags("timesheets")
               .WithSummary("Ask for the weekly timesheet for the given week.")
               .Produces<TimeSheet>(StatusCodes.Status200OK)
               .Produces<ApiError[]>(StatusCodes.Status500InternalServerError);

           app.MapGet("/timesheets/activities", GetActivities)
               .WithTags("tracking")
               .WithSummary("Get activities for connected user.")
               .Produces<Activity[]>(StatusCodes.Status200OK)
               .Produces<ApiError[]>(StatusCodes.Status500InternalServerError);

           app.MapPost("/admin/activities/{activity_name}", CreateActivity)
               .WithTags("admin")
               .WithSummary("Add or update activities.")
               .Accepts<ActivityDescription>("application/json")
               .Produces<Activity>(StatusCodes.Status200OK)
               .Produces<ApiError[]>(StatusCodes.Status400BadRequest)
               .Produces<ApiError[]>(StatusCodes.Status500InternalServerError);

           return app;
       }

       private static async Task<IResult> GetActivities(Effects.IHandleDatabaseOperations db)
       {
           var result = await db.GetAllActivities();
           if (result.IsError)
               return result.ErrorValue.ToHttpResult();

           var activities = new List<Activity>();
           foreach (var activity in result.ResultValue)
           {
               if (FSharpOption<int>.get_IsNone(activity.IdActivity))
                   return Domain.ApplicationError.NewTechnicalErr(
                           new[] { $"Activity '{activity.Name}' has no id and cannot be returned." })
                       .ToHttpResult();

               activities.Add(new Activity(activity.IdActivity.Value, activity.Name));
           }

           return Results.Ok(activities.ToArray());
       }

       private static async Task<IResult> CreateActivity(
           [FromRoute(Name = "activity_name")] string activityName,
           [FromBody] ActivityDescription body,
           Effects.IHandleDatabaseOperations db)
       {
           try
           {
               var result = await Shell.saveActivity(
                   db, activityName, body.Description, body.Tags ?? Array.Empty<string>());

               return result.ToHttpResult(created =>
               {
                   if (FSharpOption<int>.get_IsNone(created.IdActivity))
                       return Domain.ApplicationError.NewTechnicalErr(
                               new[] { $"Activity '{created.Name}' has no id and cannot be returned." })
                           .ToHttpResult();

                   return Results.Ok(new Activity(created.IdActivity.Value, created.Name));
               });
           }
           catch (Exception e)
           {
               return Results.Json(
                   new[] { new ApiError("Technical_Error", e.Message) },
                   statusCode: StatusCodes.Status500InternalServerError);
           }
       }
   }
   ```
4. Replace the whole content of `workshop/src/ClockOrDie.WebApi/Program.cs` with exactly:
   ```csharp
   using ClockOrDie.Core;
   using ClockOrDie.WebApi;
   using Scalar.AspNetCore;

   var builder = WebApplication.CreateBuilder(args);

   builder.Services.AddSingleton<Effects.IHandleDatabaseOperations, FakeDb>();
   builder.Services.AddOpenApi();

   var app = builder.Build();

   if (app.Environment.IsDevelopment())
   {
       app.MapOpenApi();
       app.MapScalarApiReference();
   }

   app.UseHttpsRedirection();
   app.MapClockOrDieEndpoints();

   app.Run();
   ```
5. Delete the old NSwag/controller artifacts:
   ```bash
   cd workshop
   rm -rf src/ClockOrDie.WebApi/Controllers
   rm -f src/ClockOrDie.WebApi/Globals.cs
   rm -f src/ClockOrDie.WebApi/AppErrorExt.cs
   rm -f nswag.json
   rm -rf api-definition
   rm -f TODO/minimal-api-plan.md
   rmdir TODO 2>/dev/null || true
   ```
6. In `workshop/paket.dependencies`, make `group WebApi` look exactly like this:
   ```
   group WebApi
   source https://api.nuget.org/v3/index.json
   framework: net10.0, netstandard2.1
   nuget Microsoft.AspNetCore.OpenApi ~> 10.0
   nuget Scalar.AspNetCore
   ```
7. In `workshop/src/ClockOrDie.WebApi/paket.references`, make the file exactly:
   ```
   FSharp.Core

   group WebApi
   Microsoft.AspNetCore.OpenApi
   Scalar.AspNetCore
   ```
8. In `workshop/src/ClockOrDie.WebApi/Properties/launchSettings.json`, replace every `"launchUrl": "swagger"` with `"launchUrl": "scalar"` (two occurrences).
9. Run:
   ```bash
   cd workshop
   dotnet paket install
   dotnet build --nologo
   ```
Verify: cd workshop && dotnet build --nologo && ! grep -RniIE 'nswag|swashbuckle|api-definition|clock-or-die\.yaml|CodeGenClockOrDie' --exclude-dir=bin --exclude-dir=obj --exclude-dir=paket-files src paket.dependencies paket.lock
Done when: the build succeeds, the grep finds no match, and `Program.cs` no longer mentions controllers, Swagger, `AddControllers`, or `MapControllers`.
If verify fails: write `.workplans/remove-nswag/problems/09-switch-to-minimal-api-and-remove-nswag.md` with the failing command and full error output, set this item's `Status:` to `failed problems/09-switch-to-minimal-api-and-remove-nswag.md`, then stop.
