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
