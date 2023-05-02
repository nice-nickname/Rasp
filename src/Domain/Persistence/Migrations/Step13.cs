using System.Data;
using Domain.Extensions;
using FluentMigrator;

namespace Domain.Persistence;

[Migration(13, "Added Class table")]
public class Step13 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(Class))
              .WithColumn(nameof(Class.Id)).AsIntPK()
              .WithColumn(nameof(Class.AuditoriumId)).AsInt32()
              .WithColumn(nameof(Class.ScheduleFormatId)).AsInt32()
              .WithColumn(nameof(Class.DisciplinePlanId)).AsInt32()
              .WithColumn(nameof(Class.Week)).AsByte()
              .WithColumn(nameof(Class.Day)).AsByte()
              .WithColumn(nameof(Class.SubGroupNo)).AsByte();

        Create.ForeignKey("FK_Class_Auditorium")
              .FromTable(nameof(Class)).ForeignColumn(nameof(Class.AuditoriumId))
              .ToTable(nameof(Auditorium)).PrimaryColumn(nameof(Auditorium.Id))
              .OnDeleteOrUpdate(Rule.Cascade);

        Create.ForeignKey("FK_Class_ScheduleFormat")
              .FromTable(nameof(Class)).ForeignColumn(nameof(Class.ScheduleFormatId))
              .ToTable(nameof(ScheduleFormat)).PrimaryColumn(nameof(ScheduleFormat.Id))
              .OnDeleteOrUpdate(Rule.Cascade);

        Create.ForeignKey("FK_Class_DisciplinePlan")
              .FromTable(nameof(Class)).ForeignColumn(nameof(Class.DisciplinePlanId))
              .ToTable(nameof(DisciplinePlan)).PrimaryColumn(nameof(DisciplinePlan.Id))
              .OnDeleteOrUpdate(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_Class_Auditorium").OnTable(nameof(Class));
        Delete.ForeignKey("FK_Class_ScheduleFormat").OnTable(nameof(Class));
        Delete.ForeignKey("FK_Class_DisciplinePlan").OnTable(nameof(Class));
        Delete.Table(nameof(Class));
    }
}
