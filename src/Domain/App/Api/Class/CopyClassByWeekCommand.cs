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

    protected override void Execute()
    {
        var matchedClasses = new List<Class>();

        var sourcePlans = Repository.Query(new Class.Where.ByWeek(SourceWeek).And(new Class.Where.ByFaculty(FacultyId)))
                                    .GroupBy(s => s.DisciplinePlanId)
                                    .ToDictionary(k => k.Key, v => v.ToList());

        Dispatcher.Push(new DeleteEntitiesByIds<Class>(Repository.Query(new Class.Where.ByWeek(DestinationWeek))
                                                                 .Select(s => s.Id)));

        var classesToPlace = Dispatcher.Query(new GetClassByWeekQuery
        {
                FacultyId = FacultyId,
                Week = DestinationWeek
        });

        foreach (var @class in classesToPlace)
        {
            if (!sourcePlans.ContainsKey(@class.DisciplinePlanId))
                continue;

            var sourceByPlan = sourcePlans[@class.DisciplinePlanId];

            if (!sourceByPlan.Any())
                continue;

            var match = sourceByPlan.FirstOrDefault(s => !@class.HasSubGroups || s.SubGroupNo == @class.SubGroupNo);

            if (match == null)
                continue;

            matchedClasses.Add(match);
            sourceByPlan.Remove(match);
        }

        // TODO 13.05.2023: Consider using bulk insert
        var i = 0;
        foreach (var match in matchedClasses)
        {
            if (i % 20 == 0)
            {
                Repository.Flush();
                Repository.Clear();
            }

            Repository.Save(new Class
            {
                    ScheduleFormatId = match.ScheduleFormatId,
                    DisciplinePlanId = match.DisciplinePlanId,
                    AuditoriumId = match.AuditoriumId,
                    SubGroupNo = match.SubGroupNo,
                    Week = DestinationWeek,
                    Day = match.Day,
            });
        }
    }

    public class Validator : AbstractValidator<CopyClassByWeekCommand>
    {
        public Validator()
        {
            RuleFor(s => s.SourceWeek).Must((command, sourceWeek) => sourceWeek > 0 && sourceWeek < command.DestinationWeek)
                                      .WithMessage(DataResources.IncorrectValue);
            RuleFor(s => s.DestinationWeek).GreaterThan(0).WithMessage(DataResources.IncorrectValue);
        }
    }
}
