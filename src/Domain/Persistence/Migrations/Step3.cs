namespace Domain.Persistence;

using System.Data;
using FluentMigrator;

[Migration(3, "Fixed Teachers table. Removed column [Teacher], added missed column [Name]")]
public class Step3 : Migration
{
    public override void Up()
    {
        if (Schema.Table(nameof(Teacher)).Column(nameof(Teacher)).Exists())
        {
            Delete.Column(nameof(Teacher)).FromTable(nameof(Teacher));
        }

        if (!Schema.Table(nameof(Teacher)).Column(nameof(Teacher.Name)).Exists())
        {
            Create.Column(nameof(Teacher.Name))
                  .OnTable(nameof(Teacher))
                  .AsString(255);
        }
    }

    public override void Down()
    {
        
    }
}
