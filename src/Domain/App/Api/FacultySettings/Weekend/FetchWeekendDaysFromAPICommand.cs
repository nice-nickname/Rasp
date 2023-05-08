using Domain.Persistence;
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

        var task = apiClient.CheckDatesRangeAsync(Start, End);
        task.Wait();
        if (!task.IsCompletedSuccessfully)
        {
            throw new ApplicationException("IsDayOff api service is not available");
        }
        var result = task.Result;

        Repository.DeleteAll<Holidays>();

        foreach (var dayOffDateTime in result.Where(dayOffDateTime => dayOffDateTime.DayType == DayType.NotWorkingDay))
        {
            Repository.Save(new Holidays
            { 
                    Date = DateOnly.FromDateTime(dayOffDateTime.DateTime)
            });
        }
    }
}
