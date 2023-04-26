using FluentMigrator;

namespace Domain.Persistence;

[Migration(9, "Added course to Group")]
public class Step9 : Migration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(Group)).Column(nameof(Group.Course)).Exists())
        {
            Create.Column(nameof(Group.Course))
                  .OnTable(nameof(Group))
                  .AsByte()
                  .WithDefaultValue(1);
        }
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}
