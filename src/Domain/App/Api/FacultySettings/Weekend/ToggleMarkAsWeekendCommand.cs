using System.Globalization;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class ToggleMarkAsWeekendCommand : CommandBase
{
    public DateTime Day { get; set; }

    protected override void Execute()
    {
        var asDateOnly = DateOnly.FromDateTime(Day);

        var day = Repository.Query<Holidays>()
                            .FirstOrDefault(s => s.Date == asDateOnly);

        if (day == null)
        {
            Repository.Save(new Holidays
            {
                    Date = asDateOnly
            });
        }
        else
        {
            Repository.Delete(day);
        }
    }
}
