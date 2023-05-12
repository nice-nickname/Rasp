using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class RescheduleDisciplinePlanByWeekCommand : CommandBase
{
    public int CurrentCountOfWeeks { get; set; }

    public int CountOfWeeks { get; set; }

    protected override void Execute()
    {
        if (CountOfWeeks < CurrentCountOfWeeks)
        {
            var weeksToDelete = Repository.Query<DisciplinePlanByWeek>()
                                          .Where(s => s.Week > CountOfWeeks)
                                          .Select(s => (object)s.Id);
            if (weeksToDelete.Any())
            {
                Repository.DeleteByIds<DisciplinePlanByWeek>(weeksToDelete);
            }
            return;
        }
        
        var allDisciplineWeeks = Repository.Query<DisciplinePlanByWeek>()
                                           .GroupBy(s => s.DisciplinePlanId);

        foreach (var dWeek in allDisciplineWeeks)
        {
            for (var i = dWeek.Max(s => s.Week) + 1; i <= CountOfWeeks; i++)
            {
                Repository.Save(new DisciplinePlanByWeek
                {
                    DisciplinePlanId = dWeek.Key,
                    AssignmentHours = 0,
                    Week = i
                });
            }
        }
    }
}
