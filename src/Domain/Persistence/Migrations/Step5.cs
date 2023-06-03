using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Persistence;

[ExcludeFromCodeCoverage]
[Migration(5, "Fixed table ScheduleFormat")]
public class Step5 : Migration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(ScheduleFormat)).Column(nameof(ScheduleFormat.Order)).Exists())
        {
            Alter.Table(nameof(ScheduleFormat))
                 .AddColumn(nameof(ScheduleFormat.Order)).AsByte();
        }
    }

    public override void Down() { }
}
