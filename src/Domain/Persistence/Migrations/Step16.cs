using Domain.Extensions;
using FluentMigrator;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Persistence;

[ExcludeFromCodeCoverage]
[Migration(16, "Added Holiday table")]
public class Step16 : Migration
{
    public override void Up()
    {
        Create.Table(nameof(Holidays))
              .WithColumn(nameof(Holidays.Id)).AsIntPK()
              .WithColumn(nameof(Holidays.Date)).AsDate()
              .WithColumn(nameof(Holidays.Name)).AsString(255).Nullable();
    }

    public override void Down()
    {
        Delete.Table(nameof(Holidays));
    }
}
