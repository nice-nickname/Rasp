using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.Api;

public class AddOrEditScheduleFormatCommand : CommandBase
{
    public int FacultyId { get; set; }

    public int ItemsCount { get; set; }

    public int CountOfWeeks { get; set; }

    public DateTime StartDate { get; set; }

    public List<ScheduleItem> Items { get; set; }

    protected override void Execute()
    {
        var existedItems = Repository.Query<ScheduleFormat>()
                                     .Where(f => f.FacultyId == FacultyId)
                                     .Select(s => s.Id)
                                     .Cast<object>();
        if (existedItems.Any())
            Repository.DeleteByIds<ScheduleFormat>(existedItems);

        Dispatcher.Push(new AddOrEditFacultySettingCommand<DateTime>
        {
                FacultyId = FacultyId,
                Type = FacultySettings.OfType.StartDate,
                Value = StartDate
        });

        Dispatcher.Push(new AddOrEditFacultySettingCommand<int>
        {
                FacultyId = FacultyId,
                Type = FacultySettings.OfType.CountOfWeeks,
                Value = CountOfWeeks
        });

        foreach (var scheduleItem in Items.Where(s => s.Order < ItemsCount))
            Repository.Save(new ScheduleFormat
            {
                    Order = scheduleItem.Order,
                    Start = scheduleItem.Start.GetValueOrDefault(),
                    End = scheduleItem.End.GetValueOrDefault(),
                    FacultyId = FacultyId
            });
    }

    public record ScheduleItem
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public TimeSpan? Start { get; set; }

        public TimeSpan? End { get; set; }
    }

    public class Validator : AbstractValidator<AddOrEditScheduleFormatCommand>
    {
        public Validator()
        {
            RuleFor(s => s.ItemsCount).GreaterThanOrEqualTo(1).WithName(DataResources.ScheduleItemsCount);
            RuleFor(s => s.CountOfWeeks).GreaterThanOrEqualTo(1).WithName(DataResources.CountOfWeeks);
            RuleFor(s => s.StartDate).NotEmpty().NotNull().WithName(DataResources.StartDate);
            RuleForEach(s => s.Items).NotEmpty().NotNull().Must((command, item) =>
            {
                var currentIndex = command.Items.IndexOf(item);

                if (!(item.Start.HasValue || item.End.HasValue))
                    return true;

                if (currentIndex < 1)
                    return true;

                return command.Items[currentIndex].Start > command.Items[currentIndex - 1].End;
            }).WithMessage(DataResources.Validation_ScheduleItemIntersectsWithPrevious);
            RuleForEach(s => s.Items).Must(item => item.Start.HasValue && item.End.HasValue && item.Start < item.End).WithMessage(DataResources.IncorrectValue);
        }
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
                                                   Order = s.Order,
                                                   Id = s.Id
                                           })
                                           .ToList();

            return new AddOrEditScheduleFormatCommand
            {
                    ItemsCount = schedulerItems.Count,
                    Items = schedulerItems,
                    StartDate = Dispatcher.Query(new GetFacultySettingQuery<DateTime> { FacultyId = FacultyId, Type = FacultySettings.OfType.StartDate }),
                    CountOfWeeks = Dispatcher.Query(new GetFacultySettingQuery<int> { FacultyId = FacultyId, Type = FacultySettings.OfType.CountOfWeeks })
            };
        }
    }
}
