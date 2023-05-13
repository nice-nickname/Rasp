using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class RescheduleScheduleFormatCommand : CommandBase
{
    public int FacultyId { get; set; }

    public List<AddOrEditScheduleFormatCommand.ScheduleItem> CurrentItems {get;set;}

    public List<AddOrEditScheduleFormatCommand.ScheduleItem> NewItems {get;set;}

    protected override void Execute()
    {
        if (NewItems.Count < CurrentItems.Count)
        {
            var toDelete = CurrentItems.Where(s => NewItems.All(q => q.Order != s.Order))
                                       .Select(s => s.Id.GetValueOrDefault())
                                       .ToList();
            
            var preferences = Repository.Query<TeacherPreferences>()
                                        .Where(s => toDelete.Contains(s.ScheduleFormatId))
                                        .Select(s => s.Id)
                                        .ToList();
            
            var classses = Repository.Query<Class>()
                                     .Where(s => toDelete.Contains(s.ScheduleFormatId))
                                     .Select(s => s.Id)
                                     .ToList();

            Dispatcher.Push(new DeleteEntitiesByIds<TeacherPreferences>(preferences));
            Dispatcher.Push(new DeleteEntitiesByIds<Class>(classses));
            Dispatcher.Push(new DeleteEntitiesByIds<ScheduleFormat>(toDelete));
        }

        foreach (var schedule in NewItems)
        {
            var scheduleItem = Repository.GetById<ScheduleFormat>(schedule.Id) ?? new ScheduleFormat();
            scheduleItem.Start = schedule.Start.GetValueOrDefault();
            scheduleItem.End = schedule.End.GetValueOrDefault();
            scheduleItem.Order = schedule.Order;
            scheduleItem.FacultyId = FacultyId;
            Repository.SaveOrUpdate(scheduleItem);
        }
    }
}
