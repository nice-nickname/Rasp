namespace Domain.Common;

public record ExportScheduleItem
{
    public int? AuditoriumId { get; set; }

    public int? TeacherId { get; set; }

    public int? GroupId { get; set; }
}

public record ZipHtmlModel
{
    public List<ExportScheduleItem> Items { get; set; }

    public int[] Weeks { get; set; }

    public int FacultyId { get; set; }
}
