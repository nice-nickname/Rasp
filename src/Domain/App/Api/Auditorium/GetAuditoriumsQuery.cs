using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api.Auditorium;

public class GetAuditoriumsQuery : QueryBase<List<GetAuditoriumsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Persistence.Auditorium>()
                         .Select(r => new Response
                         {
                                 Id = r.Id,
                                 BuildingName = r.Building.Name,
                                 DepartmentCode = r.Department != null ? r.Department.Code : string.Empty,
                                 Code = r.Code,
                                 Kinds = r.Kinds.Select(q => q.Kind)
                                          .ToList()
                         })
                         .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string DepartmentCode { get; set; }

        public string BuildingName { get; set; }

        public List<string> Kinds { get; set; }
    }
}
