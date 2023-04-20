using System.Data;
using Domain.Extensions;
using FluentMigrator;

namespace Domain.Persistence;

[Migration(6, "Added FacultySettings table")]
public class Step6 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(FacultySettings))
              .WithColumn(nameof(FacultySettings.Id)).AsIntPK()
              .WithColumn(nameof(FacultySettings.Type)).AsInt16()
              .WithColumn(nameof(FacultySettings.Value)).AsString(int.MaxValue)
              .WithColumn(nameof(FacultySettings.FacultyId)).AsInt32();

        Create.ForeignKey("FK_FacultySettings_Faculty")
              .FromTable(nameof(FacultySettings)).ForeignColumn(nameof(FacultySettings.FacultyId))
              .ToTable(nameof(Faculty)).PrimaryColumn(nameof(Faculty.Id))
              .OnDeleteOrUpdate(Rule.None);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_FacultySettings_Faculty");
        Delete.Table(nameof(FacultySettings));
    }
}
