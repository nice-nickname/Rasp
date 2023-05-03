using FluentMigrator;

namespace Domain.Persistence;

[Migration(15, "Added Type to SubDisciplineKind")]
public class Step15 : Migration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(SubDisciplineKind)).Column(nameof(SubDisciplineKind.Type)).Exists())
        {
            Create.Column(nameof(SubDisciplineKind.Type))
                  .OnTable(nameof(SubDisciplineKind))
                  .AsByte()
                  .SetExistingRowsTo((int)SubDisciplineKind.OfType.PRACTICE);
        }
    }

    public override void Down()
    {
        if (Schema.Table(nameof(SubDisciplineKind)).Column(nameof(SubDisciplineKind.Type)).Exists())
        {
            Delete.Column(nameof(SubDisciplineKind.Type))
                  .FromTable(nameof(SubDisciplineKind));
        }
    }
}
