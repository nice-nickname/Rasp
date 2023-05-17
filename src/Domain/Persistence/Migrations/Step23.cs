using FluentMigrator;

namespace Domain.Persistence;

[Migration(23, "Added Class.IsUnwanted")]
public class Step23 : Migration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(Class)).Column(nameof(Class.IsUnwanted)).Exists())
        {
            Create.Column(nameof(Class.IsUnwanted))
                  .OnTable(nameof(Class))
                  .AsBoolean()
                  .WithDefaultValue(false);
        }
    }

    public override void Down() { }
}
