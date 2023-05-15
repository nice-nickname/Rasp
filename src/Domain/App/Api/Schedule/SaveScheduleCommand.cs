using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Web;
using Resources;

namespace Domain.Api;

public class SaveScheduleCommand : CommandBase
{
    public int DisciplinePlanId { get; set; }

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
        var @class = new Class
        {
                Id = Id ?? 0,
                Week = Week,
                AuditoriumId = AuditoriumId,
                Day = Day,
                ScheduleFormatId = ScheduleFormatId,
                SubGroupNo = SubGroupNo,
                DisciplinePlanId = DisciplinePlanId
        };

        var isTeacherBusy = Repository.Query<Class>()
                                      .ToList()
                                      .Any(c => c.Plan.TeacherId == TeacherId
                                             && c.Week == Week
                                             && c.Day == Day
                                             && c.ScheduleFormatId == ScheduleFormatId);

        if (isTeacherBusy)
        {
            Result = new { CustomValidationMessage = DataResources.TeacherBusyAtThisTime };
            throw IncWebException.For<SaveScheduleCommand>(r => r.CustomValidationMessage, DataResources.TeacherBusyAtThisTime);
        }

        Repository.SaveOrUpdate(@class);
    }
}
