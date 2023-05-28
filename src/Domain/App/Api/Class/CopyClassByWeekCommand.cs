using Domain.App.Api;
using Domain.App.Common;
using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Resources;

namespace Domain.Api;

public class CopyClassByWeekCommand : CommandBase
{
    public int FacultyId { get; set; }

    public int SourceWeek { get; set; }

    public int DestinationWeek { get; set; }

    public int?[] AuditoriumIds { get; set; }

    public int?[] TeacherIds { get; set; }

    public int?[] GroupIds { get; set; }

    protected override void Execute()
    {
        var matchedClasses = new List<Class>();

        var weekends = Dispatcher.Query(new GetWeekendsForWeekQuery
                                 {
                                         FacultyId = FacultyId,
                                         StartDate = Dispatcher.Query(new GetDateFromWeekQuery { FacultyId = FacultyId, Week = DestinationWeek })
                                 })
                                 .Select(s => s.DayOfWeek)
                                 .ToHashSet();

        var sourceClasses = Repository.Query(new Class.Where.ByWeek(SourceWeek).And(new Class.Where.ByFaculty(FacultyId)))
                                    .GroupBy(s => s.DisciplinePlanId)
                                    .ToDictionary(k => k.Key, v => v.ToList());

        Dispatcher.Push(new DeleteEntitiesByIds<Class>(Repository.Query(new Class.Where.ByWeek(DestinationWeek))
                                                                 .Select(s => s.Id)));

        var classesByDisciplinePlan = Dispatcher.Query(new GetClassByWeekQuery
        {
                FacultyId = FacultyId,
                Week = DestinationWeek,
                SelectedAuditoriumIds = AuditoriumIds,
                SelectedTeacherIds = TeacherIds,
                SelectedGroupIds = GroupIds
        });

        foreach (var @class in classesByDisciplinePlan)
        {
            if (!sourceClasses.ContainsKey(@class.DisciplinePlanId))
                continue;

            var sourceByPlan = sourceClasses[@class.DisciplinePlanId];

            if (!sourceByPlan.Any())
                continue;

            var match = sourceByPlan.FirstOrDefault(s => !@class.HasSubGroups || s.SubGroupNo == @class.SubGroupNo);

            if (match == null)
                continue;

            if (weekends.Contains(match.Day))
                continue;

            sourceByPlan.Remove(match);
            match.Week = DestinationWeek;
            matchedClasses.Add(match);
        }

        Dispatcher.Push(new BulkInsertClassCommand
        {
                Classes = matchedClasses
        });
    }

    public class Validator : AbstractValidator<CopyClassByWeekCommand>
    {
        public Validator()
        {
            RuleFor(s => s.SourceWeek).Must((command, sourceWeek) => sourceWeek > 0 && sourceWeek != command.DestinationWeek)
                                      .WithMessage(DataResources.IncorrectValue);
            RuleFor(s => s.DestinationWeek).GreaterThan(0).WithMessage(DataResources.IncorrectValue);
        }
    }
}
