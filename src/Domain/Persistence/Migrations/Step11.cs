using System.Data;
using Domain.Extensions;
using FluentMigrator;

namespace Domain.Persistence;

[Migration(11, "Added DisciplinePlanByWeek")]
public class Step11 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(DisciplinePlanByWeek))
              .WithColumn(nameof(DisciplinePlanByWeek.Id)).AsIntPK()
              .WithColumn(nameof(DisciplinePlanByWeek.AssignmentHours)).AsByte()
              .WithColumn(nameof(DisciplinePlanByWeek.Week)).AsByte()
              .WithColumn(nameof(DisciplinePlanByWeek.DisciplinePlanId)).AsInt32();

        Create.ForeignKey("FK_DisciplinePlanByWeek_DisciplinePlan")
              .FromTable(nameof(DisciplinePlanByWeek)).ForeignColumn(nameof(DisciplinePlanByWeek.DisciplinePlanId))
              .ToTable(nameof(DisciplinePlan)).PrimaryColumn(nameof(DisciplinePlan.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DisciplinePlanByWeek_DisciplinePlan").OnTable(nameof(DisciplinePlanByWeek));
        Delete.Table(nameof(DisciplinePlanByWeek));
    }
}
