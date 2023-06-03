using Domain.Extensions;
using FluentMigrator;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Persistence;

[ExcludeFromCodeCoverage]
[Migration(1, "Added base tables")]
public class Step1 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(Faculty))
              .WithColumn(nameof(Faculty.Id)).AsIntPK()
              .WithColumn(nameof(Faculty.Code)).AsString(64)
              .WithColumn(nameof(Faculty.Name)).AsString(128);

        Create.Table(nameof(Department))
              .WithColumn(nameof(Department.Id)).AsIntPK()
              .WithColumn(nameof(Department.Code)).AsString(64)
              .WithColumn(nameof(Department.Name)).AsString(128)
              .WithColumn(nameof(Department.FacultyId)).AsInt32();

        Create.ForeignKey("FK_Department_Faculty")
              .FromTable(nameof(Department)).ForeignColumn(nameof(Department.FacultyId))
              .ToTable(nameof(Faculty)).PrimaryColumn(nameof(Faculty.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(Building))
              .WithColumn(nameof(Building.Id)).AsIntPK()
              .WithColumn(nameof(Building.Name)).AsString(100);

        Create.Table(nameof(Auditorium))
              .WithColumn(nameof(Auditorium.Id)).AsIntPK()
              .WithColumn(nameof(Auditorium.Code)).AsString(16)
              .WithColumn(nameof(Auditorium.DepartmentId)).AsInt32().Nullable()
              .WithColumn(nameof(Auditorium.BuildingId)).AsInt32();

        Create.ForeignKey("FK_Auditorium_Department")
              .FromTable(nameof(Auditorium)).ForeignColumn(nameof(Auditorium.DepartmentId))
              .ToTable(nameof(Department)).PrimaryColumn(nameof(Department.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.ForeignKey("FK_Auditorium_Building")
              .FromTable(nameof(Auditorium)).ForeignColumn(nameof(Auditorium.BuildingId))
              .ToTable(nameof(Building)).PrimaryColumn(nameof(Building.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(AuditoriumKind))
              .WithColumn(nameof(AuditoriumKind.Id)).AsIntPK()
              .WithColumn(nameof(AuditoriumKind.Kind)).AsString(64);

        Create.Table(nameof(AuditoriumToKinds))
              .WithColumn(nameof(AuditoriumToKinds.Id)).AsIntPK()
              .WithColumn(nameof(AuditoriumToKinds.AuditoriumId)).AsInt32()
              .WithColumn(nameof(AuditoriumToKinds.AuditoriumKindId)).AsInt32();

        Create.ForeignKey("FK_AuditoriumToKind_Auditorium")
              .FromTable(nameof(AuditoriumToKinds)).ForeignColumn(nameof(AuditoriumToKinds.AuditoriumId))
              .ToTable(nameof(Auditorium)).PrimaryColumn(nameof(Auditorium.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.ForeignKey("FK_AuditoriumToKind_AuditoriumKind")
              .FromTable(nameof(AuditoriumToKinds)).ForeignColumn(nameof(AuditoriumToKinds.AuditoriumKindId))
              .ToTable(nameof(AuditoriumKind)).PrimaryColumn(nameof(AuditoriumKind.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(Teacher))
              .WithColumn(nameof(Teacher.Id)).AsIntPK()
              .WithColumn(nameof(Teacher)).AsString(255)
              .WithColumn(nameof(Teacher.DepartmentId)).AsInt32();

        Create.ForeignKey("FK_Teacher_Department")
              .FromTable(nameof(Teacher)).ForeignColumn(nameof(Teacher.DepartmentId))
              .ToTable(nameof(Department)).PrimaryColumn(nameof(Department.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(Group))
              .WithColumn(nameof(Group.Id)).AsIntPK()
              .WithColumn(nameof(Group.Code)).AsString(32)
              .WithColumn(nameof(Group.DepartmentId)).AsInt32();

        Create.ForeignKey("FK_Group_Department")
              .FromTable(nameof(Group)).ForeignColumn(nameof(Group.DepartmentId))
              .ToTable(nameof(Department)).PrimaryColumn(nameof(Department.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(ScheduleFormat))
              .WithColumn(nameof(ScheduleFormat.Id)).AsIntPK()
              .WithColumn(nameof(ScheduleFormat.Start)).AsTime()
              .WithColumn(nameof(ScheduleFormat.End)).AsTime()
              .WithColumn(nameof(ScheduleFormat.FacultyId)).AsInt32();

        Create.ForeignKey("FK_ScheduleFormat_Faculty")
              .FromTable(nameof(ScheduleFormat)).ForeignColumn(nameof(ScheduleFormat.FacultyId))
              .ToTable(nameof(Faculty)).PrimaryColumn(nameof(Faculty.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.Table(nameof(ScheduleFormat));
        Delete.Table(nameof(Group));
        Delete.Table(nameof(Teacher));
        Delete.Table(nameof(AuditoriumToKinds));
        Delete.Table(nameof(AuditoriumKind));
        Delete.Table(nameof(Auditorium));
        Delete.Table(nameof(Building));
        Delete.Table(nameof(Department));
        Delete.Table(nameof(Faculty));
    }
}
