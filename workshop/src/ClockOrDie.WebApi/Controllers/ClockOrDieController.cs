using ClockOrDie.WebApi.Controllers.CodeGen;
using Microsoft.AspNetCore.Mvc;
using ClockOrDie.Core;
using Microsoft.FSharp.Core;


namespace ClockOrDie.WebApi.Controllers;

public class ClockOrDieController : ICodeGenClockOrDieController
{
    private readonly Effects.IHandleDatabaseOperations _db;

    public ClockOrDieController(Effects.IHandleDatabaseOperations db)
    {
        _db = db;
    }

    public async Task<IActionResult> SaveActivitiesTrackingAsync(IEnumerable<TimeSheetCell> content)
    {
        var r = new ObjectResult("Not implemented yet !")
            { StatusCode = StatusCodes.Status501NotImplemented };
        return await ValueTask.FromResult(r);
    }

    public async Task<ActionResult<TimeSheet>> GetTrackingForCurrentWeekAsync()
    {
        var r = new ObjectResult("Not implemented yet!")
            { StatusCode = StatusCodes.Status501NotImplemented };
        return await ValueTask.FromResult(r);
    }

    public async Task<ActionResult<TimeSheet>> GetTrackingForWeekAsync(int weekNumber, int weekYear)
    {
        var r = new ObjectResult("Not implemented yet!")
            { StatusCode = StatusCodes.Status501NotImplemented };
        return await ValueTask.FromResult(r);
    }

    public async Task<ActionResult<ICollection<Activity>>> GetActivitiesForUserAsync()
    {
        try
        {
            var result = await _db.GetAllActivities();
            if (result.IsError)
                return result.ErrorValue.ToActionResult();

            var activities = new List<Activity>();
            foreach (var e in result.ResultValue)
            {
                if (FSharpOption<int>.get_IsNone(e.IdActivity))
                    return Domain.ApplicationError.NewTechnicalErr(
                        new[] { $"Activity '{e.Name}' has no id and cannot be returned." }).ToActionResult();

                activities.Add(new Activity { ActivityId = e.IdActivity.Value, ActivityLabel = e.Name });
            }

            return activities.ToArray();
        }
        catch (Exception e)
        {
            var r = new ObjectResult(e.Message)
                { StatusCode = StatusCodes.Status500InternalServerError };
            return await ValueTask.FromResult(r);
        }
    }

    public async Task<ActionResult<Activity>> CreateActivityAsync(string activityName, ActivityDescription body)
    {
        try
        {
            var result = await Shell.saveActivity(_db, activityName, body.Description, body.Tags);

            if (result.IsError)
                return result.ErrorValue.ToActionResult();
        
            return new Activity
            {
                ActivityId = result.ResultValue.IdActivity.Value,
                ActivityLabel = result.ResultValue.Name
            };
        }
        catch (Exception e)
        {
            var r = new ObjectResult(e.Message)
                { StatusCode = StatusCodes.Status500InternalServerError };
            return await ValueTask.FromResult(r);
        }
    }
}