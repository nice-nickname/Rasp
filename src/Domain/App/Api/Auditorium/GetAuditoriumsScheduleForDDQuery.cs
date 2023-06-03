using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetAuditoriumsScheduleForDDQuery : QueryBase<List<KeyValueVm>>
{
    public int Week { get; set; }

    public int ScheduleFormatId { get; set; }

    public int SubDisciplineId { get; set; }

    public int? AuditoriumId { get; set; }

    public int? GroupId { get; set; }

    public DayOfWeek Day { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        var subDiscipline = Repository.GetById<SubDiscipline>(SubDisciplineId);

        var preferredKindIds = subDiscipline.AuditoriumKinds.Select(r => r.Id).ToList();
        var preferredBySelf = subDiscipline.Auditoriums.Select(r => r.Id).ToList();

        var preferredByKind = Repository.Query<AuditoriumToKinds>()
                                        .Where(s => preferredKindIds.Contains(s.AuditoriumKindId))
                                        .Select(s => s.AuditoriumId);

        var busyAuditoriums = Repository.Query<Class>()
                                        .Where(r => r.Day == Day
                                                  && r.ScheduleFormatId == ScheduleFormatId
                                                 && r.Week == Week
                                                 && r.AuditoriumId != AuditoriumId)
                                        .Select(r => r.AuditoriumId)
                                        .ToList();

        var capacity = subDiscipline.IsParallelHours
                ? subDiscipline.Discipline.Groups.Sum(s => s.StudentCount)
                : Repository.GetById<Group>(GroupId).StudentCount;

        var result = Repository.Query<Auditorium>();

        if (preferredBySelf.Any())
        {
            result = result.Where(s => preferredBySelf.Contains(s.Id));
        }

        if (preferredByKind.Any())
        {
            result = result.Where(s => s.Capacity >= capacity && preferredByKind.Contains(s.Id));
        }
        else
        {
            result = result.Where(s => s.Capacity >= capacity);
        }

        return result.Select(r => new KeyValueVm
                     {
                             Text = $"{r.Building.Name}-{r.Code}",
                             Value = r.Id.ToString(),
                             CssClass = busyAuditoriums.Contains(r.Id) ? "disabled" : string.Empty
                     })
                     .ToList();
    }
}
