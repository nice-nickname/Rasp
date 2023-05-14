using Domain.Api;

namespace UI.Common.Models;

public class SchedulePageModel
{
    public List<GetScheduleByWeekQuery.Response> Items { get; set; }

    public GetScheduleFormatQuery.Response Format { get; set; }

    public string Title { get; set; }

    public int ActiveWeek { get; set; }
}
