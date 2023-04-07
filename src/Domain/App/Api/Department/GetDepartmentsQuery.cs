using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetDepartmentsQuery : QueryBase<List<GetDepartmentsQuery.Response>>
{
    public int FacultyId { get; set; }

    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Department>()
                         .Where(s => s.FacultyId == FacultyId)
                         .Select(s => new Response
                         {
                            Id = s.Id,
                            Code = s.Code,
                            Name = s.Name
                         })
                         .ToList();
    }

    public record Response
    {
        public int? Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}