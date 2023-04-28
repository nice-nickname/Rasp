using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetDisciplineTypesForDDQuery : QueryBase<List<KeyValueVm>>
{
    public int SelectedId { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        return Repository.Query<DisciplineKind>()
                         .Select(s => new KeyValueVm(s.Id, s.Name, s.Id == SelectedId))
                         .ToList();
    }
}
