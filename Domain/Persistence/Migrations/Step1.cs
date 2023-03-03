using System.Data;
using FluentMigrator;

namespace Domain.Migrations;

[Migration(1, "First migration")]
public class Step1 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(Department))
              .WithColumn(nameof(Department.Id)).AsInt32().PrimaryKey().Identity()
              .WithColumn(nameof(Department.Name)).AsString(100)
              .WithColumn(nameof(Department.Code)).AsString(15);

        Create.Table(nameof(Teacher))
              .WithColumn(nameof(Teacher.Id)).AsInt32().PrimaryKey().Identity()
              .WithColumn(nameof(Teacher.Name)).AsString(150)
              .WithColumn(nameof(Teacher.DepartmentId)).AsInt32();

        Create.ForeignKey("FK_Teacher_Department")
              .FromTable(nameof(Teacher)).ForeignColumn(nameof(Teacher.DepartmentId))
              .ToTable(nameof(Department)).PrimaryColumn(nameof(Department.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(AuditoriumAccessoriesKind))
              .WithColumn(nameof(AuditoriumAccessoriesKind.Id)).AsInt32().PrimaryKey()
              .WithColumn(nameof(AuditoriumAccessoriesKind.Name)).AsString(30);

        Create.Table(nameof(AuditoriumAccessory))
              .WithColumn(nameof(AuditoriumAccessory.Id)).AsInt32().PrimaryKey().Identity()
              .WithColumn(nameof(AuditoriumAccessory.Kinds)).AsInt64();

        Create.Table(nameof(Auditorium))
              .WithColumn(nameof(Auditorium.Id)).AsInt32().PrimaryKey().Identity()
              .WithColumn(nameof(Auditorium.Code)).AsString(10)
              .WithColumn(nameof(Auditorium.DepartmentId)).AsInt32()
              .WithColumn(nameof(Auditorium.Accessories)).AsInt32();

        Create.ForeignKey("FK_Auditorium_AuditoriumAccessory")
              .FromTable(nameof(Auditorium)).ForeignColumn(nameof(Auditorium.Accessories))
              .ToTable(nameof(AuditoriumAccessory)).PrimaryColumn(nameof(AuditoriumAccessory.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.ForeignKey("FK_Auditorium_Department")
              .FromTable(nameof(Auditorium)).ForeignColumn(nameof(Auditorium.DepartmentId))
              .ToTable(nameof(Department)).PrimaryColumn(nameof(Department.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.Table(nameof(Teacher));
        Delete.Table(nameof(Auditorium));
        Delete.Table(nameof(AuditoriumAccessory));
        Delete.Table(nameof(AuditoriumAccessoriesKind));
        Delete.Table(nameof(Department));
    }
}