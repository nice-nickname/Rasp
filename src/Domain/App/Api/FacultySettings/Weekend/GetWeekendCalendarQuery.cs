using System.Globalization;
using Domain.App.Api;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetWeekendCalendarQuery : QueryBase<GetWeekendCalendarQuery.Response>
{
    private static readonly Func<DateTime, int> _weekProjector = d => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d,
                                                                                                                       CalendarWeekRule.FirstFourDayWeek,
                                                                                                                       DayOfWeek.Monday);

    public int FacultyId { get; set; }

    protected override Response ExecuteResult()
    {
        var days = new List<DayItem>();

        var countOfWeeks = Dispatcher.Query(new GetFacultySettingQuery<int>
        {
                FacultyId = FacultyId,
                Type = FacultySettings.OfType.CountOfWeeks
        });
        
        var start = Dispatcher.Query(new GetFacultySettingQuery<DateTime>
        {
                FacultyId = FacultyId,
                Type = FacultySettings.OfType.StartDate
        });
        
        var end = Dispatcher.Query(new GetDateFromWeekQuery
        {
                FacultyId = FacultyId,
                Week = countOfWeeks
        }).AddDays(6);

        var holidayDays = Repository.Query<Holidays>()
                                    .Select(s => s.Date)
                                    .ToList();
        while (start <= end)
        {
            days.Add(new DayItem
            {
                    Day = start,
                    Type = start.DayOfWeek == DayOfWeek.Sunday 
                            ? DayItem.WeekendType.Weekend 
                            : holidayDays.Contains(DateOnly.FromDateTime(start))
                                    ? DayItem.WeekendType.Holiday 
                                    : DayItem.WeekendType.None
            });
            start = start.AddDays(1);
        }

        return new Response
        {
                Months = days.GroupBy(s => s.Day.Month)
                             .Select(month => new MonthItem
                             {
                                     Weeks = month.GroupBy(m => _weekProjector(m.Day))
                                                  .Select(week => new WeekItem
                                                  {
                                                          Days = week.Select(day => new DayItem
                                                                     {
                                                                             Day = day.Day,
                                                                             Type = day.Type
                                                                     })
                                                                     .ToList()
                                                  })
                                                  .ToList(),
                                     Name = DateTimeFormatInfo.CurrentInfo.GetMonthName(month.Key)
                             })
                             .ToList()
        };
    }

    public record Response
    {
        public List<MonthItem> Months { get; set; }
    }

    public record MonthItem
    {
        public string Name { get; set; }

        public List<WeekItem> Weeks { get; set; }
    }

    public record WeekItem
    {
        public List<DayItem> Days { get; set; }
    }

    public record DayItem
    {
        public enum WeekendType
        {
            None,

            Weekend,

            Holiday
        }

        public WeekendType Type { get; set; }

        public DateTime Day { get; set; }
    }
}
