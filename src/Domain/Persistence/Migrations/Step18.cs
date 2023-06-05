using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Persistence;

[ExcludeFromCodeCoverage]
[Migration(18, "Added column IsParallelHours to SubDiscipline")]
public class Step18 : Migration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(SubDiscipline)).Column(nameof(SubDiscipline.IsParallelHours)).Exists())
        {
            Create.Column(nameof(SubDiscipline.IsParallelHours))
                  .OnTable(nameof(SubDiscipline))
                  .AsBoolean()
                  .WithDefaultValue(false);
        }
    }

    public override void Down() { }
}
