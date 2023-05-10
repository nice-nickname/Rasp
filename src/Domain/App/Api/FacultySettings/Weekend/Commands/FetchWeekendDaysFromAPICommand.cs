using Domain.Persistence;
using Incoding.Core.Block.Logging;
using Incoding.Core.Block.Logging.Core;
using Incoding.Core.CQRS.Core;
using isdayoff;
using isdayoff.Contract;

namespace Domain.Api;

public class FetchWeekendDaysFromAPICommand : CommandBase
{
    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    protected override void Execute()
    {
        var apiClient = new IsDayOff(IsDayOffSettings.Build
                                                     .UseDefaultCountry(Country.Russia)
                                                     .UseSixDaysWorkWeek()
                                                     .Create());

        var dateSegments = new List<Tuple<DateTime, DateTime>>();

        if (Start.Year != End.Year)
        {
            var firstYearEnd = new DateTime(Start.Year, 12, 31);

            var secondYearStart = new DateTime(End.Year, 1, 1);

            dateSegments.Add(new Tuple<DateTime, DateTime>(Start, firstYearEnd));
            dateSegments.Add(new Tuple<DateTime, DateTime>(End, secondYearStart));
        }
        else
        {
            dateSegments.Add(new Tuple<DateTime, DateTime>(Start, End));
        }
        foreach (var (start, end) in dateSegments)
        {
            try
            {
                Dispatcher.Push(new DeleteWeekendsByDatesCommand
                {
                        Start = start,
                        End = end,
                });

                var task = apiClient.CheckDatesRangeAsync(start, end);

                foreach (var dayOffDateTime in task.Result.Where(dayOffDateTime => dayOffDateTime.DayType == DayType.NotWorkingDay))
                {
                    Repository.Save(new Holidays
                    {
                            Date = DateOnly.FromDateTime(dayOffDateTime.DateTime)
                    });
                }
            }
            catch (Exception e)
            {
                LoggingFactory.Instance.LogMessage(LogType.Trace, $"Failed to load weekends from isdayoff api for Start={start} End={End}");
                LoggingFactory.Instance.LogMessage(LogType.Trace, $"isdayoff responded with exception: {e.Message}");
                LoggingFactory.Instance.LogException(LogType.Trace, e);
            }
        }
    }
}
