using Microsoft.FSharp.Core;
using ClockOrDie.Core;

namespace ClockOrDie.WebApi;

public class FakeDb : Effects.IHandleDatabaseOperations
{
    private readonly HashSet<Domain.Activity> _activities = new();
    
    public async Task<FSharpResult<IEnumerable<Domain.Activity>, Domain.ApplicationError>> GetAllActivities()
    {
        var result = FSharpResult<IEnumerable<Domain.Activity>, Domain.ApplicationError>.NewOk(_activities.ToArray());
        return await ValueTask.FromResult(result);
    }

    public async Task<FSharpResult<Domain.Activity, Domain.ApplicationError>> CreateActivity(Domain.Activity item)
    {
        if (FSharpOption<DateTime>.get_IsSome(item.ModifiedAt))
            return FSharpResult<Domain.Activity, Domain.ApplicationError>.NewError(
                Domain.ApplicationError.NewTechnicalErr(new[] { "Cannot create activity with a modified field already set" }));

        FSharpResult<Domain.Activity, Domain.ApplicationError> result;
        if (_activities.Any(e => e.IdActivity.Equals(item.IdActivity)))
        {
            result =
                FSharpResult<Domain.Activity, Domain.ApplicationError>.NewError(Domain.ApplicationError.NewBusinessErr(
                    new[] { "Cannot create an activity which already exists !" }));
        } else
        {
            var newId = _activities
                .Where(e => FSharpOption<int>.get_IsSome(e.IdActivity))
                .Select(e => e.IdActivity.Value).DefaultIfEmpty(0).Max() + 1;
            var newItem =
                new Domain.Activity(FSharpOption<int>.Some(newId), item.Name,
                    item.Tags, item.Description,item.CreatedAt,item.ModifiedAt);
            _activities.Add(newItem);
            result = FSharpResult<Domain.Activity, Domain.ApplicationError>.NewOk(newItem);
        }

        return await ValueTask.FromResult(result);
    }

    public async Task<FSharpResult<Domain.Activity, Domain.ApplicationError>> UpdateActivity(Domain.Activity item)
    {
        FSharpResult<Domain.Activity, Domain.ApplicationError> result;
        if (!_activities.Any(e => e.IdActivity.Equals(item.IdActivity)))
        {
            result =
                FSharpResult<Domain.Activity, Domain.ApplicationError>
                    .NewError(Domain.ApplicationError.NewTechnicalErr(new[] { "Cannot find activity to update !" }));
        }
        else
        {
            _activities.RemoveWhere(e => e.IdActivity.Equals(item.IdActivity));
            _activities.Add(item);
            result = FSharpOption<DateTime>.get_IsSome(item.ModifiedAt)
                ? FSharpResult<Domain.Activity, Domain.ApplicationError>.NewOk(item)
                : FSharpResult<Domain.Activity, Domain.ApplicationError>.NewError(
                    Domain.ApplicationError.NewTechnicalErr(
                        new[] { $"Cannot update activity, {nameof(item.ModifiedAt)} not set to a value!" }));
        }

        return await ValueTask.FromResult(result);
    }
}