using System.Data;
using Domain.Extensions;
using FluentMigrator;

namespace Domain.Persistence;

[Migration(10, "Added DisciplinePlan")]
public class Step10 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(DisciplinePlan))
              .WithColumn(nameof(DisciplinePlan.Id)).AsIntPK()
              .WithColumn(nameof(DisciplinePlan.Week)).AsByte()
              .WithColumn(nameof(DisciplinePlan.SubGroups)).AsByte()
              .WithColumn(nameof(DisciplinePlan.SubDisciplineId)).AsInt32()
              .WithColumn(nameof(DisciplinePlan.TeacherId)).AsInt32()
              .WithColumn(nameof(DisciplinePlan.GroupId)).AsInt32();

        Create.ForeignKey("FK_DisciplinePlan_SubDiscipline")
              .FromTable(nameof(DisciplinePlan)).ForeignColumn(nameof(DisciplinePlan.SubDisciplineId))
              .ToTable(nameof(SubDiscipline)).PrimaryColumn(nameof(SubDiscipline.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.ForeignKey("FK_DisciplinePlan_Teacher")
              .FromTable(nameof(DisciplinePlan)).ForeignColumn(nameof(DisciplinePlan.TeacherId))
              .ToTable(nameof(Teacher)).PrimaryColumn(nameof(Teacher.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DisciplinePlan_SubDiscipline").OnTable(nameof(DisciplinePlan));
        Delete.ForeignKey("FK_DisciplinePlan_Teacher").OnTable(nameof(DisciplinePlan));
        Delete.Table(nameof(DisciplinePlan));
    }
}
