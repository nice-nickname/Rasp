using Incoding.Core.CQRS.Core;

namespace Domain.Api.AuditoriumKind;

public class GetAuditoriumKindsQuery : QueryBase<List<GetAuditoriumKindsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Persistence.AuditoriumKind>()
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
