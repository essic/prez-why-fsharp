Status: todo
Goal: Restore the old controller's exception handling on `GET /timesheets/activities` and the old 400 (not 404) behaviour for non-integer week route values.
Files: `workshop/src/ClockOrDie.WebApi/Endpoints.cs`
Steps:
1. In `workshop/src/ClockOrDie.WebApi/Endpoints.cs`, replace the whole `GetActivities` method with exactly:
   ```csharp
       private static async Task<IResult> GetActivities(Effects.IHandleDatabaseOperations db)
       {
           try
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
           catch (Exception e)
           {
               return Results.Json(
                   new[] { new ApiError("Technical_Error", e.Message) },
                   statusCode: StatusCodes.Status500InternalServerError);
           }
       }
   ```
2. In the same file, change the route template:
   ```
   app.MapGet("/timesheets/week/{week_number:int}/{week_year:int}",
   ```
   to:
   ```
   app.MapGet("/timesheets/week/{week_number}/{week_year}",
   ```
   Keep the `[FromRoute(Name = ...)] int` parameters unchanged; without the constraint, a non-integer value now fails binding with 400 instead of missing the route with 404.
3. Run `dotnet build --nologo` from `workshop`.
Verify: cd workshop && dotnet build --nologo && test "$(grep -c 'catch (Exception e)' src/ClockOrDie.WebApi/Endpoints.cs)" -eq 2 && ! grep -q 'week_number:int' src/ClockOrDie.WebApi/Endpoints.cs
Done when: the build succeeds, `Endpoints.cs` contains two `catch (Exception e)` blocks, and the week route has no `:int` constraints.
If verify fails: write `.workplans/remove-nswag/problems/10-restore-endpoint-parity.md` with the failing command and full error output, set this item's `Status:` to `failed problems/10-restore-endpoint-parity.md`, then stop.
