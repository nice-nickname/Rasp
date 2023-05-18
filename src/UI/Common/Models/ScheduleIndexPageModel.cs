namespace UI.Common.Models;

public class ScheduleIndexPageModel
{
    public enum EntityType
    {
        AUDITORIUM,

        TEACHER,

        GROUP
    }

    public EntityType? Type { get; set; }

    public int? Week { get; set; }

    public int? AuditoriumId { get; set; }

    public int? TeacherId { get; set; }

    public int? GroupId { get; set; }
}
