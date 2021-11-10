using ClockOrDie.Core;
using Microsoft.AspNetCore.Mvc;

namespace ClockOrDie.WebApi;

public static class AppErrorExt
{
    public static ActionResult ToActionResult(this Domain.AppError? err)
    {
        switch (err?.Tag ?? -1)
        {
            case Domain.AppError.Tags.TechnicalErr:
                var techError = (Domain.AppError.TechnicalErr)err!;
                return new ObjectResult(techError.Item)
                    { StatusCode = StatusCodes.Status500InternalServerError };
            case Domain.AppError.Tags.BusinessErr:
                var bizError = (Domain.AppError.BusinessErr)err!;
                return new ObjectResult(bizError.Item)
                    { StatusCode = StatusCodes.Status400BadRequest };
            default:
                return new ObjectResult("Unknown error occured")
                    { StatusCode = StatusCodes.Status501NotImplemented };
        }
    }
}