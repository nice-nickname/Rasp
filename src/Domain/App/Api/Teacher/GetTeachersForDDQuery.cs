using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetTeachersForDDQuery : QueryBase<List<DropDownItem>>
{
    public int FacultyId { get; set; }

    public List<int>? SelectedIds { get; set; }

    protected override List<DropDownItem> ExecuteResult()
    {
        SelectedIds ??= new List<int>();
        return Repository.Query(new Share.Where.ByFacultyThoughDepartment<Teacher>(FacultyId))
                         .Select(s => new DropDownItem(s.Id, s.Name, SelectedIds.Contains(s.Id), s.Department.Code, ""))
                         .ToList();
    }
}
