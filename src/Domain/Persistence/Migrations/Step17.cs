using FluentMigrator;

namespace Domain.Persistence;

[Migration(17, "Added type to DisciplineKind")]
public class Step17 : Migration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(DisciplineKind)).Column(nameof(DisciplineKind.Type)).Exists())
        {
            Create.Column(nameof(DisciplineKind.Type))
                  .OnTable(nameof(DisciplineKind))
                  .AsByte()
                  .Nullable();

            Execute.Sql($"update DisciplineKind set [Type] = {(int)SubDisciplineKind.OfType.EXAM} where Id=3");
        }
    }

    public override void Down() { }
}
