using System.Data;
using Domain.Extensions;
using FluentMigrator;

namespace Domain.Persistence;

[Migration(4, "Added DisciplineKind")]
public class Step4 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(DisciplineKind))
              .WithColumn(nameof(DisciplineKind.Id)).AsIntPK()
              .WithColumn(nameof(DisciplineKind.Name)).AsString(128);

        Create.ForeignKey("FK_Discipline_DisciplineKind")
              .FromTable(nameof(Discipline)).ForeignColumn(nameof(Discipline.KindId))
              .ToTable(nameof(DisciplineKind)).PrimaryColumn(nameof(DisciplineKind.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(SubDisciplineKind))
              .WithColumn(nameof(SubDisciplineKind.Id)).AsIntPK()
              .WithColumn(nameof(SubDisciplineKind.Name)).AsString(128)
              .WithColumn(nameof(SubDisciplineKind.Code)).AsString(32);

        Create.ForeignKey("FK_SubDiscipline_SubDisciplineKind")
              .FromTable(nameof(SubDiscipline)).ForeignColumn(nameof(SubDiscipline.KindId))
              .ToTable(nameof(SubDisciplineKind)).PrimaryColumn(nameof(SubDisciplineKind.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_Discipline_DisciplineKind");
        Delete.ForeignKey("FK_SubDiscipline_SubDisciplineKind");
        Delete.Table(nameof(DisciplineKind));
        Delete.Table(nameof(SubDisciplineKind));
    }
}
