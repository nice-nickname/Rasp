using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

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
