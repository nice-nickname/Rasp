using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetWeekendsForWeekQuery : QueryBase<List<DateOnly>>
{
    private readonly Func<DateTime, DayOfWeek, DateTime> getDay = (startDate, day) =>
    {
        int diff = day - startDate.DayOfWeek;
        return startDate.AddDays(diff).Date;
    };

    public int FacultyId { get; set; }

    public DateTime StartDate { get; set; }

    protected override List<DateOnly> ExecuteResult()
    {
        var scheduleFormat = Dispatcher.Query(new GetScheduleFormatQuery { FacultyId = FacultyId });

        var startDate = DateOnly.FromDateTime(StartDate);
        var endDate = DateOnly.FromDateTime(StartDate.AddDays(7));

        var response = Repository.Query<Holidays>()
                                 .Where(r => r.Date >= startDate
                                          && r.Date < endDate)
                                 .Select(r => r.Date)
                                 .ToList();

        for (int i = 1; i < 7; i++)
        {
            var currentDate = this.getDay(StartDate, (DayOfWeek)i);
            var isBlocked = response.Contains(DateOnly.FromDateTime(currentDate))
                         || currentDate < scheduleFormat.StartDate
                         || currentDate > scheduleFormat.EndDate;

            if (isBlocked && !response.Contains(DateOnly.FromDateTime(currentDate)))
                response.Add(DateOnly.FromDateTime(currentDate));
        }

        return response;
    }
}
