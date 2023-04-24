using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetGroupsForDDQuery : QueryBase<List<KeyValueVm>>
{
    public int FacultyId { get; set; }

    public int[]? SelectedIds { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        SelectedIds ??= Array.Empty<int>();
        return Repository.Query(new Share.Where.ByFacultyThoughDepartment<Group>(FacultyId))
                         .Select(s => new KeyValueVm(s.Id, s.Code, SelectedIds.Contains(s.Id)))
                         .ToList();
    }
}
