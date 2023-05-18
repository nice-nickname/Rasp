namespace Domain.Common;

public record ExportScheduleItem
{
    public int? AuditoriumId { get; set; }

    public int? TeacherId { get; set; }

    public int? GroupId { get; set; }
}

public record ZipHtmlModel
{
    public List<int>? Teachers { get; set; } = new();

    public List<int>? Groups { get; set; } = new();

    public List<int>? Auditoriums { get; set; } = new();

    public int StartWeek { get; set; }

    public int EndWeek { get; set; }

    public int FacultyId { get; set; }
}
