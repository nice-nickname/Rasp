using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetAuditoriumKindsQuery : QueryBase<List<GetAuditoriumKindsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<AuditoriumKind>()
                         .Select(r => new Response
                         {
                                 Id = r.Id,
                                 Kind = r.Kind
                         })
                         .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public string Kind { get; set; }
    }
}
