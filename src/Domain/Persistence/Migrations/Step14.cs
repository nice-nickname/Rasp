using System.Drawing;
using FluentMigrator;

namespace Domain.Persistence;

[Migration(14, "Added Color to SubDisciplineKind")]
public class Step14 : Migration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(SubDisciplineKind)).Column(nameof(SubDisciplineKind.Color)).Exists())
        {
            Create.Column(nameof(SubDisciplineKind.Color))
                  .OnTable(nameof(SubDisciplineKind))
                  .AsAnsiString(12)
                  .SetExistingRowsTo("#FF0000");
        }
    }

    public override void Down()
    {
        if (Schema.Table(nameof(SubDisciplineKind)).Column(nameof(SubDisciplineKind.Color)).Exists())
        {
            Delete.Column(nameof(SubDisciplineKind.Color))
                  .FromTable(nameof(SubDisciplineKind));
        }
    }
}
