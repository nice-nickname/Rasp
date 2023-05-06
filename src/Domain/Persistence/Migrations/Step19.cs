using FluentMigrator;

namespace Domain.Persistence;

[Migration(19, "Class.AuditoriumId is now Nullable")]
public class Step19 : Migration
{
    public override void Up()
    {
        Alter.Column(nameof(Class.AuditoriumId))
             .OnTable(nameof(Class))
             .AsInt32()
             .Nullable();
    }

    public override void Down() { }
}
