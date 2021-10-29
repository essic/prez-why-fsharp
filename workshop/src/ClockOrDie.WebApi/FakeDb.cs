using Microsoft.FSharp.Core;
using ClockOrDie.Core;

namespace ClockOrDie.WebApi;

public class FakeDb : Effects.IHandleDatabaseOperations
{
    private readonly HashSet<Domain.Activity> _activities = new();
    
    public async Task<FSharpResult<IEnumerable<Domain.Activity>, Domain.AppError>> GetAllActivities()
    {
        var r =  _activities.ToList();
        var result = FSharpResult<IEnumerable<Domain.Activity>, Domain.AppError>.NewOk(r);
        return await ValueTask.FromResult(result);
    }

    public async Task<FSharpResult<Domain.Activity, Domain.AppError>> CreateActivity(Domain.Activity item)
    {
        FSharpResult<Domain.Activity, Domain.AppError> result;
        if (_activities.Contains(item))
        {
            result =
                FSharpResult<Domain.Activity, Domain.AppError>.NewError(Domain.AppError.NewBusinessErr(new[] { "NO NO NO !" }));
        } else
        {
            var newId = _activities
                .Where(e => FSharpOption<int>.get_IsSome(e.IdActivity))
                .Select(e => e.IdActivity.Value).DefaultIfEmpty(0).Max() + 1;
            var newItem =
                new Domain.Activity(FSharpOption<int>.Some(newId), item.Name, item.Tags, item.Description);
            _activities.Add(newItem);
            result = FSharpResult<Domain.Activity, Domain.AppError>.NewOk(newItem);
        }

        return await ValueTask.FromResult(result);
    }

    public async Task<FSharpResult<Domain.Activity, Domain.AppError>> UpdateActivity(Domain.Activity item)
    {
        FSharpResult<Domain.Activity, Domain.AppError> result;
        if (!_activities.Any(e => e.IdActivity.Equals(item.IdActivity)))
        {
            result =
                FSharpResult<Domain.Activity, Domain.AppError>.NewError(Domain.AppError.NewBusinessErr(new[] { "NO NO NO !" }));
        }
        else
        {
            _activities.RemoveWhere(e => e.IdActivity.Equals(item.IdActivity));
            _activities.Add(item);
            result = FSharpResult<Domain.Activity, Domain.AppError>.NewOk(item);
        }

        return await ValueTask.FromResult(result);
    }
}