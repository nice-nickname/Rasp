using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api.Schedule;

public class SaveScheduleCommand : CommandBase
{
    public int DisciplinePlanId { get; set; }

    public int Week { get; set; }

    public int ScheduleFormatId { get; set; }

    public int SubGroupNo { get; set; }

    public int? Id { get; set; }

    public int? AuditoriumId { get; set; }

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

        Repository.SaveOrUpdate(@class);
    }
}
