Status: superseded by 09 (work landed in e76065c)
Goal: Add the request/response records that describe the API, and a helper that turns F# `Result` values into HTTP responses.
Files: `workshop/src/ClockOrDie.WebApi/Dtos.cs`, `workshop/src/ClockOrDie.WebApi/ApiResults.cs`
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
3. Do not touch `Program.cs` or the old controllers in this item.
4. Run `dotnet build --nologo` from `workshop`.
Verify: cd workshop && dotnet build --nologo
Done when: the build succeeds with the two new files compiled.
If verify fails: write `.workplans/remove-nswag/problems/02-add-dtos-and-http-mapping.md` with the failing command and full error output, set this item's `Status:` to `failed problems/02-add-dtos-and-http-mapping.md`, then stop.
