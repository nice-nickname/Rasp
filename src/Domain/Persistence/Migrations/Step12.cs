using System.Data;
using System.Diagnostics.CodeAnalysis;
using Domain.Extensions;
using FluentMigrator;

namespace Domain.Persistence;

[ExcludeFromCodeCoverage]
[Migration(12, "Added teacher preferences")]
public class Step12 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(TeacherPreferences))
              .WithColumn(nameof(TeacherPreferences.Id)).AsIntPK()
              .WithColumn(nameof(TeacherPreferences.TeacherId)).AsInt32()
              .WithColumn(nameof(TeacherPreferences.ScheduleFormatId)).AsInt32()
              .WithColumn(nameof(TeacherPreferences.Type)).AsByte();

        Create.ForeignKey("FK_TeacherPreferences_Teacher")
              .FromTable(nameof(TeacherPreferences)).ForeignColumn(nameof(TeacherPreferences.TeacherId))
              .ToTable(nameof(Teacher)).PrimaryColumn(nameof(Teacher.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.ForeignKey("FK_TeacherPreferences_ScheduleFormat")
              .FromTable(nameof(TeacherPreferences)).ForeignColumn(nameof(TeacherPreferences.ScheduleFormatId))
              .ToTable(nameof(ScheduleFormat)).PrimaryColumn(nameof(ScheduleFormat.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_TeacherPreferences_Teacher").OnTable(nameof(TeacherPreferences));
        Delete.ForeignKey("FK_TeacherPreferences_ScheduleFormat").OnTable(nameof(TeacherPreferences));
        Delete.Table(nameof(TeacherPreferences));
    }
}
