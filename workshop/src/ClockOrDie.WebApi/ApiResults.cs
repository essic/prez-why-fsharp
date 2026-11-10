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
