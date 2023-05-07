using Domain.Api;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.App.Api;

public class GetDateFromWeekQuery : QueryBase<DateTime>
{
    public int FacultyId { get; set; }

    public int Week { get; set; }

    protected override DateTime ExecuteResult()
    {
        var start = Dispatcher.Query(new GetFacultySettingQuery<DateTime>
        {
                Type = FacultySettings.OfType.StartDate,
                FacultyId = FacultyId
        });
        
        if (Week <= 1)
        {
            return start;
        }

        start = start.AddDays(7 * (Week - 1));
        start = start.DayOfWeek == DayOfWeek.Sunday
                ? start.AddDays(-6)
                : start.AddDays((int)DayOfWeek.Monday - (int)start.DayOfWeek);

        return start;
    }
}
