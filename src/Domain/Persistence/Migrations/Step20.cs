using System.Data;
using Domain.Extensions;
using FluentMigrator;

namespace Domain.Persistence;

[Migration(20, "Created tables for ")]
public class Step20 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(SubDisciplineAuditoriums))
              .WithColumn(nameof(SubDisciplineAuditoriums.Id)).AsIntPK()
              .WithColumn(nameof(SubDisciplineAuditoriums.AuditoriumId)).AsInt32()
              .WithColumn(nameof(SubDisciplineAuditoriums.SubDisciplineId)).AsInt32();

        Create.ForeignKey("FK_SubDisciplineAuditoriums_Auditorium")
              .FromTable(nameof(SubDisciplineAuditoriums)).ForeignColumn(nameof(SubDisciplineAuditoriums.AuditoriumId))
              .ToTable(nameof(Auditorium)).PrimaryColumn(nameof(Auditorium.Id))
              .OnDeleteOrUpdate(Rule.Cascade);

        Create.ForeignKey("FK_SubDisciplineAuditoriums_SubDiscipline")
              .FromTable(nameof(SubDisciplineAuditoriums)).ForeignColumn(nameof(SubDisciplineAuditoriums.SubDisciplineId))
              .ToTable(nameof(SubDiscipline)).PrimaryColumn(nameof(SubDiscipline.Id))
              .OnDeleteOrUpdate(Rule.None);

        Create.Table(nameof(SubDisciplineAuditoriumKinds))
              .WithColumn(nameof(SubDisciplineAuditoriumKinds.Id)).AsIntPK()
              .WithColumn(nameof(SubDisciplineAuditoriumKinds.AuditoriumKindId)).AsInt32()
              .WithColumn(nameof(SubDisciplineAuditoriumKinds.SubDisciplineId)).AsInt32();

        Create.ForeignKey("FK_SubDisciplineAuditoriumKinds_AuditoriumKind")
              .FromTable(nameof(SubDisciplineAuditoriumKinds)).ForeignColumn(nameof(SubDisciplineAuditoriumKinds.AuditoriumKindId))
              .ToTable(nameof(AuditoriumKind)).PrimaryColumn(nameof(AuditoriumKind.Id))
              .OnDeleteOrUpdate(Rule.Cascade);

        Create.ForeignKey("FK_SubDisciplineAuditoriumKinds_SubDiscipline")
              .FromTable(nameof(SubDisciplineAuditoriumKinds)).ForeignColumn(nameof(SubDisciplineAuditoriumKinds.SubDisciplineId))
              .ToTable(nameof(SubDiscipline)).PrimaryColumn(nameof(SubDiscipline.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_SubDisciplineAuditoriums_Auditorium").OnTable(nameof(SubDisciplineAuditoriums));
        Delete.ForeignKey("FK_SubDisciplineAuditoriums_SubDiscipline").OnTable(nameof(SubDisciplineAuditoriums));
        Delete.Table(nameof(SubDisciplineAuditoriums));

        Delete.ForeignKey("FK_SubDisciplineAuditoriumKinds_AuditoriumKind").OnTable(nameof(SubDisciplineAuditoriumKinds));
        Delete.ForeignKey("FK_SubDisciplineAuditoriumKinds_SubDiscipline").OnTable(nameof(SubDisciplineAuditoriumKinds));
        Delete.Table(nameof(SubDisciplineAuditoriumKinds));
    }
}
