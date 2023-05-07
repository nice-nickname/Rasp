using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetScheduleFormatQuery : QueryBase<GetScheduleFormatQuery.Response>
{
    public int FacultyId { get; set; }

    protected override Response ExecuteResult()
    {
        var schedulerItems = Repository.Query<ScheduleFormat>()
                                       .Where(s => s.FacultyId == FacultyId)
                                       .OrderBy(s => s.Order)
                                       .Select(s => new AddOrEditScheduleFormatCommand.ScheduleItem
                                       {
                                               Start = s.Start,
                                               End = s.End,
                                               Order = s.Order,
                                               Id = s.Id
                                       })
                                       .ToList();

        return new Response
        {
                ItemsCount = schedulerItems.Count,
                Items = schedulerItems,
                StartDate = Dispatcher.Query(new GetFacultySettingCommand<DateTime> { FacultyId = FacultyId, Type = FacultySettings.OfType.StartDate }),
                CountOfWeeks = Dispatcher.Query(new GetFacultySettingCommand<int> { FacultyId = FacultyId, Type = FacultySettings.OfType.CountOfWeeks })
        };
    }

    public class Response
    {
        public int ItemsCount { get; set; }

        public int CountOfWeeks { get; set; }

        public DateTime StartDate { get; set; }

        public List<AddOrEditScheduleFormatCommand.ScheduleItem>? Items { get; set; }
    }
}
