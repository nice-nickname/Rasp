using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetSubDisciplineKindsForDDQuery : QueryBase<List<KeyValueVm>>
{
    protected override List<KeyValueVm> ExecuteResult()
    {
        return Repository.Query<SubDisciplineKind>()
                         .Select(s => new KeyValueVm(s.Id, s.Name))
                         .ToList();
    }
}
