using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Builders.Delete;

namespace Domain.Extentions;

public static partial class Extention
{
    public static ICreateTableColumnOptionOrWithColumnSyntax AsIntPK(this ICreateTableColumnAsTypeSyntax builder)
    {
        return builder.AsInt32().PrimaryKey().Identity();
    }
}