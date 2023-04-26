using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetDisciplinesQuery : QueryBase<List<GetDisciplinesQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Discipline>()
                         .Select(s => new Response
                         {
                             Id = s.Id,
                             Name = s.Code,
                             Kind = ""
                         })
                         .ToList();

    }

    public record Response
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Kind { get; set; }
    }
}
