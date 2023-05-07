using Domain.Api;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.App.Api;

public class GetWeekFromDateQuery : QueryBase<int>
{
    public int FacultyId { get; set; }

    public DateTime Date { get; set; }

    protected override int ExecuteResult()
    {
        var start = Dispatcher.Query(new GetFacultySettingQuery<DateTime>
        {
                Type = FacultySettings.OfType.StartDate,
                FacultyId = FacultyId
        });

        if (Date <= start)
        {
            return 1;
        }

        var week = 1;
        start = (start.DayOfWeek == DayOfWeek.Sunday
                ? start.AddDays(-6)
                : start.AddDays((int)DayOfWeek.Monday - (int)start.DayOfWeek)).AddDays(7);

        while (Date >= start)
        {
            week++;
            start = start.AddDays(7);
        }

        return week;
    }
}
