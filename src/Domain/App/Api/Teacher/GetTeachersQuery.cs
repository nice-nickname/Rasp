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

public class GetTeachersQuery : QueryBase<List<GetTeachersQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        var data = Repository.Query<Teacher>()
                             .Select(r => new
                             {
                                     r.Id,
                                     r.Name,
                                     r.DepartmentId,
                                     DepartmentCode = r.Department.Code,
                                     DepartmentName = r.Department.Name
                             })
                             .ToList();

        var response = new List<Response>();

        foreach (var teacher in data)
        {
            var name = teacher.Name.Split(" ");

            response.Add(new Response
            {
                    Id = teacher.Id,
                    Name = teacher.Name,
                    DepartmentId = teacher.DepartmentId,
                    DepartmentCode = teacher.DepartmentCode,
                    DepartmentName = teacher.DepartmentName,
                    Initials = name.Length > 2 ? $"{name[0]} {name[1].First()}. {name[2].First()}." : teacher.Name
            });
        }

        return response;
    }

    public class Response
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public string Initials { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }
    }
}
