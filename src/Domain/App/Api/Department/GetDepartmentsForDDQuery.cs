using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetDepartmentsForDDQuery : QueryBase<List<KeyValueVm>>
{
    public int FacultyId { get; set; }

    public int SelectedId { get; set; }

    public string? Optional { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        var list = new List<KeyValueVm>();

        if (!string.IsNullOrWhiteSpace(Optional))
            list.Add(new KeyValueVm(null, Optional));

        list.AddRange(Repository.Query(new Share.Where.ByFaculty<Department>(FacultyId))
                                .Select(s => new KeyValueVm(s.Id, s.Code, s.Id == SelectedId)));

        return list;
    }
}
