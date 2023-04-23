using FluentMigrator;

namespace Domain.Persistence;

[Migration(8, "Added capacity to auditoriums and StudentCount to group")]
public class Step8 : Migration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(Auditorium)).Column(nameof(Auditorium.Capacity)).Exists())
        {
            Create.Column(nameof(Auditorium.Capacity))
                  .OnTable(nameof(Auditorium))
                  .AsInt16()
                  .WithDefaultValue(0);
        }

        if (!Schema.Table(nameof(Group)).Column(nameof(Group.StudentCount)).Exists())
        {
            Create.Column(nameof(Group.StudentCount))
                  .OnTable(nameof(Group))
                  .AsInt16()
                  .WithDefaultValue(0);
        }
    }

    public override void Down() { }
}
