using System.Data;
using Domain.Extentions;
using FluentMigrator;
using Incoding.Core;

namespace Domain.Persistence;

[Migration(1, "Added tables for ScheduleTable")]
public class Step1 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(ScheduleTable))
              .WithColumn(nameof(ScheduleTable.Id)).AsIntPK()
              .WithColumn(nameof(ScheduleTable.StartTime)).AsTime()
              .WithColumn(nameof(ScheduleTable.EndTime)).AsTime();

        Create.Table(nameof(Department))
              .WithColumn(nameof(Department.Id)).AsIntPK()
              .WithColumn(nameof(Department.Name)).AsString(50);

        Create.Table(nameof(Building))
              .WithColumn(nameof(Building.Id)).AsIntPK()
              .WithColumn(nameof(Building.Name)).AsString(10);

        Create.Table(nameof(Auditorium))
              .WithColumn(nameof(Auditorium.Id)).AsIntPK()
              .WithColumn(nameof(Auditorium.Code)).AsString(30)
              .WithColumn(nameof(Auditorium.DepartmentId)).AsInt32()
              .WithColumn(nameof(Auditorium.BuildingId)).AsInt32();

        Create.ForeignKey("FK_Auditorium_Department")
              .FromTable(nameof(Auditorium)).ForeignColumn(nameof(Auditorium.DepartmentId))
              .ToTable(nameof(Department)).PrimaryColumn(nameof(Department.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.ForeignKey("FK_Auditorium_Building")
              .FromTable(nameof(Auditorium)).ForeignColumn(nameof(Auditorium.BuildingId))
              .ToTable(nameof(Building)).PrimaryColumn(nameof(Building.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(Teacher))
              .WithColumn(nameof(Teacher.Id)).AsIntPK()
              .WithColumn(nameof(Teacher.Name)).AsString(128)
              .WithColumn(nameof(Teacher.DepartmentId)).AsInt32();

        Create.ForeignKey("FK_Teacher_Department")
              .FromTable(nameof(Teacher)).ForeignColumn(nameof(Teacher.DepartmentId))
              .ToTable(nameof(Department)).PrimaryColumn(nameof(Department.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.Table(nameof(Building));
        Delete.Table(nameof(Department));
        Delete.Table(nameof(ScheduleTable));
        Delete.Table(nameof(Auditorium));
    }
}