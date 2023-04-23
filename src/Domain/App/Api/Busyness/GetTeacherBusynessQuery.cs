using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class SaveTeacherBusynessCommand : CommandBase
{
    public List<Item> Items { get; set; }

    protected override void Execute()
    {
        throw new NotImplementedException();
    }

    public class Item
    {
        public string Reason { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}

public class GetTeacherBusynessQuery : QueryBase<List<GetTeacherBusynessQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        // TODO 23.04.2023: Пока что показывает всех преподавателей ?!
        return Repository.Query<TeacherBusyness>()
                         .Select(s => new Response
                         {
                                 Start = s.Start,
                                 End = s.End,
                                 Id = s.Id,
                                 Reason = s.Reason,
                         })
                         .ToList();
    }

    public record Response
    {
        public int Id { get; set; }

        public string Reason { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int TeacherId { get; set; }

    }
}
