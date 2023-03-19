using Domain.Extentions;
using FluentMigrator;

namespace Domain.Persistence;

[Migration(3, "Added tables for ScheduleTable")]
public class Step3 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(ScheduleTable))
              .WithColumn(nameof(ScheduleTable.Id)).AsIntPK()
              .WithColumn(nameof(ScheduleTable.StartTime)).AsTime()
              .WithColumn(nameof(ScheduleTable.EndTime)).AsTime();
    }

    public override void Down()
    {
        Delete.Table(nameof(ScheduleTable));
    }
}