using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetDepartmentsQuery : QueryBase<List<GetDepartmentsQuery.Response>>
{
    public int FacultyId { get; set; }

    protected override List<Response> ExecuteResult()
    {
        return Repository.Query(new Share.Where.ByFaculty<Department>(FacultyId))
                         .Select(s => new Response
                         {
                                 Id = s.Id,
                                 Code = s.Code,
                                 Name = s.Name
                         })
                         .OrderBy(s => s.Code)
                         .ToList();
    }

    public record Response
    {
        public int? Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}
