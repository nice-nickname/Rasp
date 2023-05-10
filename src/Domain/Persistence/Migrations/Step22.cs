using FluentMigrator;

namespace Domain.Persistence;

[Migration(22, "Added TeacherPreferences.Day")]
public class Step22 : Migration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(TeacherPreferences)).Column(nameof(TeacherPreferences.Day)).Exists())
        {
            Create.Column(nameof(TeacherPreferences.Day))
                  .OnTable(nameof(TeacherPreferences))
                  .AsByte()
                  .NotNullable();
        }
    }

    public override void Down() { }
}
