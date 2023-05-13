using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetAuditoriumsScheduleForDDQuery : QueryBase<List<KeyValueVm>>
{
    public int Capacity { get; set; }

    public int Week { get; set; }

    public int Order { get; set; }

    public int? AuditoriumId { get; set; }

    public DayOfWeek Day { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        var scheduledAuditoriums = Repository.Query<Class>()
                                             .Where(r => r.Day == Day
                                                      && r.ScheduleFormat.Order == Order
                                                      && r.Week == Week
                                                      && r.AuditoriumId != AuditoriumId)
                                             .Select(r => r.AuditoriumId)
                                             .ToList();

        return Repository.Query<Auditorium>()
                         .Where(r => r.Capacity >= Capacity)
                         .Select(r => new KeyValueVm
                         {
                                 Text = $"{r.Building.Name}-{r.Code}",
                                 Value = r.Id.ToString(),
                                 CssClass = scheduledAuditoriums.Contains(r.Id) ? "disabled" : string.Empty
                         })
                         .ToList();
    }
}
