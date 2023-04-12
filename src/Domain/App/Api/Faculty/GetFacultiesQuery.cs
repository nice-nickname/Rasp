using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetFacultiesQuery : QueryBase<List<GetFacultiesQuery.Response>>
{
    public int? FacultyId { get; set; }

    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Faculty>()
                         .Select(s => new Response
                         {
                             Id = s.Id,
                             Code = s.Code,
                             Name = s.Name,
                         })
                         .OrderBy(s => s.Code)
                         .ToList();
    }

    public record Response
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
