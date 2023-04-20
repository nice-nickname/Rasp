using FluentMigrator;

namespace Domain.Persistence;

[Migration(5, "Fixed table ScheduleFormat")]
public class Step5 : Migration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(ScheduleFormat)).Column(nameof(ScheduleFormat)).Exists())
        {
            Alter.Table(nameof(ScheduleFormat))
                 .AddColumn(nameof(ScheduleFormat.Order)).AsByte();
        }
    }

    public override void Down() { }
}
