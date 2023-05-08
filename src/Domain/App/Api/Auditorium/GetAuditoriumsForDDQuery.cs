using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetAuditoriumsForDDQuery : QueryBase<List<KeyValueVm>>
{
    protected override List<KeyValueVm> ExecuteResult()
    {
        return Repository.Query<Auditorium>()
                         .Select(r => new KeyValueVm
                         {
                                 Text = $"{r.Building.Name}-{r.Code}",
                                 Value = r.Id.ToString()
                         })
                         .ToList();
    }
}
