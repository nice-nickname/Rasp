using Domain.Common;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetAuditoriumsForSelectQuery : QueryBase<List<DropDownItem>>
{
    public List<int>? SelectedIds { get; set; }

    protected override List<DropDownItem> ExecuteResult()
    {
        SelectedIds ??= new List<int>();
        return Repository.Query<Auditorium>()
                         .Select(s => new DropDownItem(s.Id, $"{s.Building.Name}-{s.Code}", SelectedIds.Contains(s.Id), s.Building.Name, s.Department.Code))
                         .ToList();
    }
}
