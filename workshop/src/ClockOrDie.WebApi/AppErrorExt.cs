using ClockOrDie.Core;
using Microsoft.AspNetCore.Mvc;

namespace ClockOrDie.WebApi;

public static class AppErrorExt
{
    public static ActionResult ToActionResult(this Domain.ApplicationError? err)
    {
        switch (err?.Tag ?? -1)
        {
            case Domain.ApplicationError.Tags.TechnicalErr:
                var techError = (Domain.ApplicationError.TechnicalErr)err!;
                return new ObjectResult(techError.Item)
                    { StatusCode = StatusCodes.Status500InternalServerError };
            case Domain.ApplicationError.Tags.BusinessErr:
                var bizError = (Domain.ApplicationError.BusinessErr)err!;
                return new ObjectResult(bizError.Item)
                    { StatusCode = StatusCodes.Status400BadRequest };
            default:
                return new ObjectResult("Unknown error occured")
                    { StatusCode = StatusCodes.Status501NotImplemented };
        }
    }
}