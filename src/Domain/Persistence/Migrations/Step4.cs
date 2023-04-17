using System.Data;
using Domain.Extensions;
using FluentMigrator;

namespace Domain.Persistence;

[Migration(4, "Added DisciplineKinds")]
public class Step4 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(DisciplineKinds))
              .WithColumn(nameof(DisciplineKinds.Id)).AsIntPK()
              .WithColumn(nameof(DisciplineKinds.Name)).AsString(128)
              .WithColumn(nameof(DisciplineKinds.Code)).AsString(32);

        Create.ForeignKey("FK_Discipline_DisciplineKind")
              .FromTable(nameof(Discipline)).ForeignColumn(nameof(Discipline.KindId))
              .ToTable(nameof(DisciplineKinds)).PrimaryColumn(nameof(DisciplineKinds.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(SubDisciplineKinds))
              .WithColumn(nameof(SubDisciplineKinds.Id)).AsIntPK()
              .WithColumn(nameof(SubDisciplineKinds.Name)).AsString(128)
              .WithColumn(nameof(SubDisciplineKinds.Code)).AsString(32);

        Create.ForeignKey("FK_SubDiscipline_SubDisciplineKind")
              .FromTable(nameof(SubDiscipline)).ForeignColumn(nameof(SubDiscipline.KindId))
              .ToTable(nameof(SubDisciplineKinds)).PrimaryColumn(nameof(SubDisciplineKinds.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_Discipline_DisciplineKind");
        Delete.ForeignKey("FK_SubDiscipline_SubDisciplineKind");
        Delete.Table(nameof(DisciplineKinds));
        Delete.Table(nameof(SubDisciplineKinds));
    }
}
