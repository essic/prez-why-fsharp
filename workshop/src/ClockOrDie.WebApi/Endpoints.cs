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
