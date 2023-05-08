using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetBuildingsForDDQuery : QueryBase<List<KeyValueVm>>
{
    protected override List<KeyValueVm> ExecuteResult()
    {
        return Repository.Query<Building>()
                         .Select(r => new KeyValueVm
                         {
                                 Value = r.Id.ToString(),
                                 Text = r.Name
                         })
                         .ToList();
    }
}
