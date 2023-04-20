using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class AddOrEditScheduleFormatCommand : CommandBase
{
    public int FacultyId { get; set; }

    public int ItemsCount { get; set; }

    public List<ScheduleItem> Items { get; set; }

    protected override void Execute()
    {
        var existedItems = Repository.Query<ScheduleFormat>()
                                     .Where(f => f.FacultyId == FacultyId)
                                     .Select(s => s.Id)
                                     .Cast<object>();
        if (existedItems.Any())
        {
            Repository.DeleteByIds<ScheduleFormat>(existedItems);
        }

        foreach (var scheduleItem in Items.Where(s => s.Order < ItemsCount))
        {
            Repository.Save(new ScheduleFormat
            { 
                Order = scheduleItem.Order,
                Start = scheduleItem.Start.GetValueOrDefault(),
                End = scheduleItem.End.GetValueOrDefault(),
                FacultyId = FacultyId
            });
        }
    }

    public record ScheduleItem
    {
        public int Order { get; set; }

        public TimeSpan? Start { get; set; }

        public TimeSpan? End { get; set; }
    }

    public class AsQuery : QueryBase<AddOrEditScheduleFormatCommand>
    {
        public int FacultyId { get; set; }

        protected override AddOrEditScheduleFormatCommand ExecuteResult()
        {
            var schedulerItems = Repository.Query<ScheduleFormat>()
                                           .Where(s => s.FacultyId == FacultyId)
                                           .OrderBy(s => s.Order)
                                           .Select(s => new ScheduleItem
                                           {
                                               Start = s.Start,
                                               End = s.End,
                                               Order = s.Order
                                           })
                                           .ToList();

            return new AddOrEditScheduleFormatCommand
            {
                    ItemsCount = schedulerItems.Count,
                    Items = schedulerItems
            };
        }
    }
}
