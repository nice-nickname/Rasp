using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetAuditoriumKindsForDDQuery : QueryBase<List<KeyValueVm>>
{
    public int[]? Ids { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        Ids ??= Array.Empty<int>();
        return Repository.Query<AuditoriumKind>()
                         .Select(r => new KeyValueVm
                         {
                                 Text = r.Kind,
                                 Value = r.Id.ToString(),
                                 Selected = Ids.Contains(r.Id)
                         })
                         .ToList();
    }
}
