using ClockOrDie.WebApi.Controllers.CodeGen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ClockOrDie.Core;


namespace ClockOrDie.WebApi.Controllers;

public class CustomValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context) { }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.HasJsonContentType())
            context.Result = new UnsupportedMediaTypeResult();

        if (context.ModelState.IsValid) return;
        var details = new ValidationProblemDetails(context.ModelState);

        var errors =
            details.Errors.Select(e => new Error
            {
                Code = "Invalid_Payload_Format",
                Message = string.Join('\n', e.Value)
            }).ToArray();

        var res =
            new BadRequestObjectResult(errors)
                { StatusCode = StatusCodes.Status400BadRequest};
        context.Result = res;
    }
}

public class ClockOrDieController : ICodeGenClockOrDieController
{
    private readonly Effects.IHandleDatabaseOperations _db;

    public ClockOrDieController(Effects.IHandleDatabaseOperations db)
    {
        _db = db;
    }

    public async Task<IActionResult> SaveActivitiesTrackingAsync(IEnumerable<TimeSheetCell> body)
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResult<TimeSheet>> GetTrackingForCurrentWeekAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResult<TimeSheet>> GetTrackingForWeekAsync(int week_number, int week_year)
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResult<ICollection<Activity>>> GetActivitiesForUserAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResult<Activity>> CreateActivityAsync(string activity_name, ActivityDescription body)
    {
        var result = await Shell.saveActivity(_db, activity_name, body.Description, body.Tags);
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
                var techError = (Domain.AppError.TechnicalErr)result.ErrorValue;
                return new ObjectResult(techError.Item) 
                    {StatusCode = StatusCodes.Status500InternalServerError};
            case Domain.AppError.Tags.BusinessErr:
                var bizError = (Domain.AppError.BusinessErr)result.ErrorValue;
                return new ObjectResult(bizError.Item)
                    { StatusCode = StatusCodes.Status400BadRequest};
            default:
                return new ObjectResult("Unknown error occured") 
                    { StatusCode = StatusCodes.Status501NotImplemented };
        }
    }
}