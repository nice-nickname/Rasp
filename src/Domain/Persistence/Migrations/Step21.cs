using System.Diagnostics.CodeAnalysis;
using FluentMigrator;

namespace Domain.Persistence;

[ExcludeFromCodeCoverage]
[Migration(21, "Discipline.DepartmentId is now nullable")]
public class Step21 : Migration
{
    public override void Up()
    {
        Alter.Column(nameof(Discipline.DepartmentId))
             .OnTable(nameof(Discipline))
             .AsInt32()
             .Nullable();
    }

    public override void Down() { }
}
