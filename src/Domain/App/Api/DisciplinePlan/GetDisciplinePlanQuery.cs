using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetDisciplinePlanQuery : QueryBase<List<GetDisciplinePlanQuery.Response>>
{
    public int DisciplineId { get; set; }

    protected override List<Response> ExecuteResult()
    {
        return new List<Response>();
    }

    public record Response
    {
        public int SubDisciplineId { get; set; }

        public List<Item> Items { get; set; }
    }

    public record Item
    {
        public int Hours { get; set; }

        public int Week { get; set; }
    }
}
