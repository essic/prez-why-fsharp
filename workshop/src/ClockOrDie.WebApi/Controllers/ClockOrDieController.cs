using ClockOrDie.WebApi.Controllers.CodeGen;
using Microsoft.AspNetCore.Mvc;
using ClockOrDie.Core;


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
        var r = new ObjectResult("Unknown error occured") 
            { StatusCode = StatusCodes.Status501NotImplemented };
        return await ValueTask.FromResult(r);
    }

    public async Task<ActionResult<Activity>> CreateActivityAsync(string activityName, ActivityDescription body)
    {
        var result = await Shell.saveActivity(_db, activityName, body.Description, body.Tags);
        if (result.IsOk)
        {
            return new Activity
            {
                ActivityId = result.ResultValue.IdActivity.Value,
                ActivityLabel = result.ResultValue.Name
            };
        }

        switch (result.ErrorValue?.Tag ?? -1)
        {
            case Domain.AppError.Tags.TechnicalErr:
                var techError = (Domain.AppError.TechnicalErr)result.ErrorValue!;
                return new ObjectResult(techError.Item) 
                    {StatusCode = StatusCodes.Status500InternalServerError};
            case Domain.AppError.Tags.BusinessErr:
                var bizError = (Domain.AppError.BusinessErr)result.ErrorValue!;
                return new ObjectResult(bizError.Item)
                    { StatusCode = StatusCodes.Status400BadRequest};
            default:
                return new ObjectResult("Unknown error occured") 
                    { StatusCode = StatusCodes.Status501NotImplemented };
        }
    }
}