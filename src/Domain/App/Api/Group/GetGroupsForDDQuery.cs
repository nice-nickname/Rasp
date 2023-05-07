using Domain.Common;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.Api;

public class GetGroupsForDDQuery : QueryBase<List<DropDownItem>>
{
    public int FacultyId { get; set; }

    public int[]? SelectedIds { get; set; }

    protected override List<DropDownItem> ExecuteResult()
    {
        SelectedIds ??= Array.Empty<int>();
        return Repository.Query(new Share.Where.ByFacultyThoughDepartment<Group>(FacultyId))
                         .Select(s => new DropDownItem(s.Id, s.Code, SelectedIds.Contains(s.Id), $"{s.Course} {DataResources.Course}", s.Department.Code))
                         .ToList();
    }
}
