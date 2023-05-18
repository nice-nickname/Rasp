using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetAuditoriumsScheduleForDDQuery : QueryBase<List<KeyValueVm>>
{
    public int Week { get; set; }

    public int Order { get; set; }

    public int SubDisciplineId { get; set; }

    public int? AuditoriumId { get; set; }

    public DayOfWeek Day { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        var preferredAuditoriumIds = Repository.Query<SubDisciplineAuditoriums>()
                                               .Where(r => r.SubDisciplineId == SubDisciplineId)
                                               .Select(r => r.AuditoriumId)
                                               .ToList();

        var preferredKindIds = Repository.Query<SubDisciplineAuditoriumKinds>()
                                         .Where(r => r.SubDisciplineId == SubDisciplineId)
                                         .Select(r => r.AuditoriumKindId)
                                         .ToList();

        var scheduledAuditoriums = Repository.Query<Class>()
                                             .Where(r => r.Day == Day
                                                      && r.ScheduleFormat.Order == Order
                                                      && r.Week == Week
                                                      && r.AuditoriumId != AuditoriumId)
                                             .Select(r => r.AuditoriumId)
                                             .ToList();

        var capacity = Repository.Query<Class>()
                                 .Where(r => r.Plan.SubDisciplineId == SubDisciplineId)
                                 .Sum(r => r.Plan.Group.StudentCount);

        return Repository.Query<Auditorium>()
                         .ToList()
                         .Where(r => (r.Capacity >= capacity
                                   && (preferredKindIds.Count <= 0 || r.Kinds.Select(q => q.Id).Intersect(preferredKindIds).Any()))
                                  || preferredAuditoriumIds.Contains(r.Id))
                         .Select(r => new KeyValueVm
                         {
                                 Text = $"{r.Building.Name}-{r.Code}",
                                 Value = r.Id.ToString(),
                                 CssClass = scheduledAuditoriums.Contains(r.Id) ? "disabled" : string.Empty
                         })
                         .ToList();
    }
}
