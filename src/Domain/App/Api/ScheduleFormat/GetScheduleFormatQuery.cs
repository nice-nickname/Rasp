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

        var startDate = Dispatcher.Query(new GetFacultySettingQuery<DateTime> { FacultyId = FacultyId, Type = FacultySettings.OfType.StartDate });
        var countOfWeeks = Dispatcher.Query(new GetFacultySettingQuery<int> { FacultyId = FacultyId, Type = FacultySettings.OfType.CountOfWeeks });
        var endDate = startDate.AddDays(7 * countOfWeeks);

        return new Response
        {
                ItemsCount = schedulerItems.Count,
                Items = schedulerItems,
                StartDate = startDate,
                EndDate = endDate,
                CountOfWeeks = countOfWeeks
        };
    }

    public class Response
    {
        public int ItemsCount { get; set; }

        public int CountOfWeeks { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<AddOrEditScheduleFormatCommand.ScheduleItem>? Items { get; set; }
    }
}
