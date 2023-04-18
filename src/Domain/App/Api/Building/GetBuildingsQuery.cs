using Incoding.Core.CQRS.Core;

namespace Domain.Api.Building;

public class GetBuildingsQuery : QueryBase<List<GetBuildingsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Persistence.Building>()
                         .Select(r => new Response
                         {
                                 Id = r.Id,
                                 Name = r.Name
                         })
                         .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
