using ClockOrDie.WebApi.Controllers.CodeGen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClockOrDie.WebApi.Controllers;

public class CustomValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context) { }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.Request.Method == "POST" || context.HttpContext.Request.Method == "PUT")
        {
            if (!context.HttpContext.Request.HasJsonContentType())
                context.Result = new UnsupportedMediaTypeResult();
        }

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