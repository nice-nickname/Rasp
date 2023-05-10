using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteWeekendsByDatesCommand : CommandBase
{
    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    protected override void Execute()
    {
        var holidays = Repository.Query(new Holidays.Where.BetweenDates(Start, End))
                                 .ToList();
        if (holidays.Any())
        {
            Repository.DeleteByIds<Holidays>(holidays.Select(s => (object)s.Id));
        }
    }
}
