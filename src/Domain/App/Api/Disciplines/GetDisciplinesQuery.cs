using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.Api;

public class GetDisciplinesQuery : QueryBase<List<GetDisciplinesQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Discipline>()
                         .ToList()
                         .Select(s => new Response
                         {
                                 Id = s.Id,
                                 Name = s.Name,
                                 Code = s.Code,
                                 FilterCourses = string.Join(" ", s.Groups.GroupBy(r => r.Course).Select(r => $"{r.Key} {DataResources.Course}"))
                         })
                         .ToList();
    }

    public record Response
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string FilterCourses { get; set; }
    }
}
