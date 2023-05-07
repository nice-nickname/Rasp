using System.Globalization;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class ToggleMarkAsWeekendCommand : CommandBase
{
    public string Day { get; set; }

    protected override void Execute()
    {
        var asDateOnly = DateOnly.FromDateTime(DateTime.Parse(Day, new CultureInfo("ru-RU")));

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
