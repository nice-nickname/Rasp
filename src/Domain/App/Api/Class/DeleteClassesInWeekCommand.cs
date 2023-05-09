using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteClassesInWeekCommand : CommandBase
{
    public int Week { get; set; }

    public int FacultyId { get; set; }

    public int? SelectedGroupId { get; set; }

    public int? SelectedAuditoriumId { get; set; }

    public int? SelectedTeacherId { get; set; }

    protected override void Execute()
    {
        if (!SelectedAuditoriumId.HasValue
            && !SelectedGroupId.HasValue
            && !SelectedTeacherId.HasValue)
            return;

        var scheduledList = Repository.Query<Class>()
                                      .Where(r => r.Week == Week)
                                      .ToList();

        if (SelectedGroupId.HasValue)
            scheduledList = scheduledList.Where(r => r.Plan.GroupId == SelectedGroupId).ToList();

        if (SelectedAuditoriumId.HasValue)
            scheduledList = scheduledList.Where(r => r.AuditoriumId == SelectedAuditoriumId).ToList();

        if (SelectedTeacherId.HasValue)
            scheduledList = scheduledList.Where(r => r.Plan.TeacherId == SelectedTeacherId).ToList();

        foreach (var scheduled in scheduledList)
            Dispatcher.Push(new DeleteClassCommand { Id = scheduled.Id });
    }
}
