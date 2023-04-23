using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetGroupsQuery : QueryBase<List<GetGroupsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Group>()
                         .Select(r => new Response
                         {
                                 Id = r.Id,
                                 Code = r.Code,
                                 DepartmentId = r.DepartmentId,
                                 StudentCount = r.StudentCount,
                                 DepartmentCode = r.Department.Code,
                                 DepartmentName = r.Department.Name
                         })
                         .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public int StudentCount { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }

        public string Code { get; set; }
    }
}
