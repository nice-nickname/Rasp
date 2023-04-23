using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetAuditoriumsQuery : QueryBase<List<GetAuditoriumsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Auditorium>()
                         .Select(r => new Response
                         {
                                 Id = r.Id,
                                 Capacity = r.Capacity,
                                 BuildingName = r.Building.Name,
                                 DepartmentCode = r.Department != null ? r.Department.Code : string.Empty,
                                 Code = r.Code,
                                 Kinds = Repository.GetById<Auditorium>(r.Id).Kinds.ToList()
                         })
                         .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public int Capacity { get; set; }

        public string Code { get; set; }

        public string DepartmentCode { get; set; }

        public string BuildingName { get; set; }

        public List<AuditoriumKind> Kinds { get; set; }
    }
}
