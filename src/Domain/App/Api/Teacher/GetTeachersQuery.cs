using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetTeachersQuery : QueryBase<List<GetTeachersQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Teacher>()
                         .Select(r => new Response
                         {
                                 Id = r.Id,
                                 Name = r.Name,
                                 DepartmentId = r.DepartmentId,
                                 DepartmentCode = r.Department.Code,
                                 DepartmentName = r.Department.Name
                         })
                         .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }
    }
}
