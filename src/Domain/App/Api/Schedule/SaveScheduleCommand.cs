using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.Api;

public class SaveScheduleCommand : CommandBase
{
    public int DisciplinePlanId { get; set; }

    public int SubDisciplineId { get; set; }

    public int Week { get; set; }

    public int ScheduleFormatId { get; set; }

    public int SubGroupNo { get; set; }

    public int TeacherId { get; set; }

    public int? Id { get; set; }

    public int? AuditoriumId { get; set; }

    public string CustomValidationMessage { get; set; }

    public DayOfWeek Day { get; set; }

    protected override void Execute()
    {
        var @class = Repository.GetById<Class>(Id) ?? new Class();

        var scheduledClasses = Repository.Query<Class>()
                                         .ToList()
                                         .Where(c => c.Plan.TeacherId == TeacherId
                                                  && c.Week == Week
                                                  && c.Day == Day
                                                  && c.ScheduleFormatId == ScheduleFormatId)
                                         .ToList();

        var isTeacherBusy = scheduledClasses.Any(c => c.Plan.SubDisciplineId != SubDisciplineId);

        if (isTeacherBusy)
        {
            Result = new { CustomValidationMessage = DataResources.TeacherBusyAtThisTime };
            return;
        }

        int? auditoriumId = null;

        if (!@class.AuditoriumId.HasValue)
        {
            auditoriumId = scheduledClasses.FirstOrDefault(c => c.Plan.SubDisciplineId == SubDisciplineId)?.AuditoriumId ?? AuditoriumId;
        }
        else
        {
            var classes = Repository.Query<Class>()
                                    .ToList()
                                    .Where(c => c.Plan.SubDisciplineId == SubDisciplineId);

            auditoriumId = AuditoriumId;

            foreach (var scheduled in classes)
            {
                scheduled.AuditoriumId = auditoriumId;
            }
        }

        @class.Week = Week;
        @class.AuditoriumId = auditoriumId;
        @class.Day = Day;
        @class.ScheduleFormatId = ScheduleFormatId;
        @class.SubGroupNo = SubGroupNo;
        @class.DisciplinePlanId = DisciplinePlanId;

        Repository.SaveOrUpdate(@class);
    }
}
