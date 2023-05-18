using Domain.Api;

namespace Domain.Common;

public class ExportPageModel
{
    public List<GetScheduleByWeekQuery.Response> Items { get; set; }

    public GetScheduleFormatQuery.Response Format { get; set; }

    public string Title { get; set; }

    public int ActiveWeek { get; set; }

    public DateTime ExportDate { get; set; }

    public DateTime StartWeekDate { get; set; }
}


public class SchedulePageModel
{
    public List<GetScheduleByWeekQuery.Response> Items { get; set; }

    public GetScheduleFormatQuery.Response Format { get; set; }

    public string Title { get; set; }

    public int ActiveWeek { get; set; }
}
