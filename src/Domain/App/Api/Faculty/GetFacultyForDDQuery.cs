using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetFacultyForDDQuery : QueryBase<List<KeyValueVm>>
{
    public int? Selected { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        var list = Repository.Query<Faculty>();

        return list.Select(r => new KeyValueVm
                   {
                           Text = $"{r.Code} - {r.Name}",
                           Value = r.Id.ToString(),
                           Selected = r.Id == Selected
                   })
                   .ToList();
    }
}
