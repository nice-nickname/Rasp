using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetBuildingsQuery : QueryBase<List<GetBuildingsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Building>()
                         .Select(r => new Response
                         {
                                 Id = r.Id,
                                 Name = r.Name
                         })
                         .OrderBy(s => s.Name)
                         .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
