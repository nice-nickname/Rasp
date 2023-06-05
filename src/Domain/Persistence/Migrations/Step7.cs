using System.Data;
using System.Diagnostics.CodeAnalysis;
using Domain.Extensions;
using FluentMigrator;

namespace Domain.Persistence;

[ExcludeFromCodeCoverage]
[Migration(7, "Added auditorium and teacher busyness")]
public class Step7 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(AuditoriumBusyness))
              .WithColumn(nameof(AuditoriumBusyness.Id)).AsIntPK()
              .WithColumn(nameof(AuditoriumBusyness.Reason)).AsString(256)
              .WithColumn(nameof(AuditoriumBusyness.Start)).AsDate()
              .WithColumn(nameof(AuditoriumBusyness.End)).AsDate()
              .WithColumn(nameof(AuditoriumBusyness.AuditoriumId)).AsInt32();

        Create.ForeignKey("FK_AuditoriumBusyness_Auditorium")
              .FromTable(nameof(AuditoriumBusyness)).ForeignColumn(nameof(AuditoriumBusyness.AuditoriumId))
              .ToTable(nameof(Auditorium)).PrimaryColumn(nameof(Auditorium.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(TeacherBusyness))
              .WithColumn(nameof(TeacherBusyness.Id)).AsIntPK()
              .WithColumn(nameof(TeacherBusyness.Reason)).AsString(256)
              .WithColumn(nameof(TeacherBusyness.Start)).AsDate()
              .WithColumn(nameof(TeacherBusyness.End)).AsDate()
              .WithColumn(nameof(TeacherBusyness.TeacherId)).AsInt32();

        Create.ForeignKey("FK_TeacherBusyness_Teacher")
              .FromTable(nameof(TeacherBusyness)).ForeignColumn(nameof(TeacherBusyness.TeacherId))
              .ToTable(nameof(Teacher)).PrimaryColumn(nameof(Teacher.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_AuditoriumBusyness_Auditorium").OnTable(nameof(AuditoriumBusyness));
        Delete.ForeignKey("FK_TeacherBusyness_Teacher").OnTable(nameof(TeacherBusyness));
        Delete.Table(nameof(TeacherBusyness));
        Delete.Table(nameof(AuditoriumBusyness));
    }
}
