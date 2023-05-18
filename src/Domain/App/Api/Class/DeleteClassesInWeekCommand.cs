using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteClassesInWeekCommand : CommandBase
{
    public int Week { get; set; }

    public int?[] SelectedGroupIds { get; set; }

    public int?[] SelectedAuditoriumIds { get; set; }

    public int?[] SelectedTeacherIds { get; set; }

    protected override void Execute()
    {
        if (SelectedGroupIds.FirstOrDefault() == null
         && SelectedAuditoriumIds.FirstOrDefault() == null
         && SelectedTeacherIds.FirstOrDefault() == null)
            return;

        var isSingle = SelectedGroupIds.Length == 1
                    && SelectedAuditoriumIds.Length == 1
                    && SelectedTeacherIds.Length == 1;

        var scheduledList = Repository.Query<Class>()
                                      .Where(r => r.Week == Week)
                                      .ToList();

        if (isSingle)
        {
            if (SelectedGroupIds.FirstOrDefault() != null)
                scheduledList = scheduledList.Where(r => r.Plan.GroupId == SelectedGroupIds.First()).ToList();

            if (SelectedAuditoriumIds.FirstOrDefault() != null)
                scheduledList = scheduledList.Where(r => r.AuditoriumId == SelectedAuditoriumIds.First()).ToList();

            if (SelectedTeacherIds.FirstOrDefault() != null)
                scheduledList = scheduledList.Where(r => r.Plan.TeacherId == SelectedTeacherIds.First()).ToList();
        }
        else
        {
            if (SelectedGroupIds.Length > 1)
                scheduledList = scheduledList.Where(r => SelectedGroupIds.Contains(r.Plan.GroupId)).ToList();

            if (SelectedAuditoriumIds.Length > 1)
                scheduledList = scheduledList.Where(r => SelectedAuditoriumIds.Contains(r.AuditoriumId)).ToList();

            if (SelectedTeacherIds.Length > 1)
                scheduledList = scheduledList.Where(r => SelectedTeacherIds.Contains(r.Plan.TeacherId)).ToList();
        }

        foreach (var scheduled in scheduledList)
            Dispatcher.Push(new DeleteClassCommand { Id = scheduled.Id });
    }
}
