using System.Data;
using Domain.Extentions;
using FluentMigrator;

namespace Domain.Persistence;

[Migration(2, "Added tables for Disciplines")]
public class Step2 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(Discipline))
              .WithColumn(nameof(Discipline.Id)).AsIntPK()
              .WithColumn(nameof(Discipline.Name)).AsString(128)
              .WithColumn(nameof(Discipline.Type)).AsByte();

        Create.Table(nameof(DisciplineGroups))
              .WithColumn(nameof(DisciplineGroups.Id)).AsIntPK()
              .WithColumn(nameof(DisciplineGroups.DisciplineId)).AsInt32()
              .WithColumn(nameof(DisciplineGroups.GroupId)).AsInt32();

        Create.ForeignKey("FK_DisciplineGroups_Discipline")
              .FromTable(nameof(DisciplineGroups)).ForeignColumn(nameof(DisciplineGroups.DisciplineId))
              .ToTable(nameof(Discipline)).PrimaryColumn(nameof(Discipline.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.ForeignKey("FK_DisciplineGroups_Group")
              .FromTable(nameof(DisciplineGroups)).ForeignColumn(nameof(DisciplineGroups.GroupId))
              .ToTable(nameof(Group)).PrimaryColumn(nameof(Group.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(DisciplineTeachers))
              .WithColumn(nameof(DisciplineTeachers.Id)).AsIntPK()
              .WithColumn(nameof(DisciplineTeachers.DisciplineId)).AsInt32()
              .WithColumn(nameof(DisciplineTeachers.TeacherId)).AsInt32();

        Create.ForeignKey("FK_DisciplineTeachers_Discipline")
              .FromTable(nameof(DisciplineTeachers)).ForeignColumn(nameof(DisciplineTeachers.DisciplineId))
              .ToTable(nameof(Discipline)).PrimaryColumn(nameof(Discipline.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.ForeignKey("FK_DisciplineTeachers_Teachers")
              .FromTable(nameof(DisciplineTeachers)).ForeignColumn(nameof(DisciplineTeachers.TeacherId))
              .ToTable(nameof(Teacher)).PrimaryColumn(nameof(Teacher.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(SubDiscipline))
              .WithColumn(nameof(SubDiscipline.Id)).AsIntPK()
              .WithColumn(nameof(SubDiscipline.Hours)).AsInt16()
              .WithColumn(nameof(SubDiscipline.DisciplineId)).AsInt32()
              .WithColumn(nameof(SubDiscipline.Type)).AsByte();

        Create.ForeignKey("FK_SubDiscipline_Discipline")
              .FromTable(nameof(SubDiscipline)).ForeignColumn(nameof(SubDiscipline.DisciplineId))
              .ToTable(nameof(Discipline)).PrimaryColumn(nameof(Discipline.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(SubDisciplineTeachers))
              .WithColumn(nameof(SubDisciplineTeachers.Id)).AsIntPK()
              .WithColumn(nameof(SubDisciplineTeachers.SubDisciplineId)).AsInt32()
              .WithColumn(nameof(SubDisciplineTeachers.TeacherId)).AsInt32();

        Create.ForeignKey("FK_SubDisciplineTeachers_SubDiscipline")
              .FromTable(nameof(SubDisciplineTeachers)).ForeignColumn(nameof(SubDisciplineTeachers.SubDisciplineId))
              .ToTable(nameof(SubDiscipline)).PrimaryColumn(nameof(SubDiscipline.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.ForeignKey("FK_SubDisciplineTeachers_Teacher")
              .FromTable(nameof(SubDisciplineTeachers)).ForeignColumn(nameof(SubDisciplineTeachers.TeacherId))
              .ToTable(nameof(Teacher)).PrimaryColumn(nameof(Teacher.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.Table(nameof(SubDisciplineTeachers));
        Delete.Table(nameof(DisciplineTeachers));
        Delete.Table(nameof(DisciplineGroups));
        Delete.Table(nameof(SubDiscipline));
        Delete.Table(nameof(Discipline));
    }
}