using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetTeachersQuery : QueryBase<List<GetTeachersQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Teacher>()
                         .Select(teacher => new Response
                         {
                                 Id = teacher.Id,
                                 Name = teacher.Name,
                                 DepartmentId = teacher.DepartmentId,
                                 DepartmentCode = teacher.Department.Code,
                                 DepartmentName = teacher.Department.Name,
                                 Initials = teacher.ShortName
                         })
                         .OrderBy(s => s.Name)
                         .ToList();
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
