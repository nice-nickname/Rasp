using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetDisciplinePlanQuery : QueryBase<GetDisciplinePlanQuery.Response>
{
    protected override Response ExecuteResult()
    {
        throw new NotImplementedException();
    }

    public record Response
    {
       public int? SubDisciplineId { get; set; }

       public int WeeksCount { get; set; }

       public List<int> AssignmentsByWeek { get; set; }
    }

}
