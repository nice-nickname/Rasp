using FluentMigrator.Builders.Create.Table;

namespace Domain.Extensions;

using System.ComponentModel;

public static partial class Extension
{
    public static string ToRelative(this string uri)
    {
        var a = new Uri(uri);
        return a.LocalPath + a.Query;
    }

    public static ICreateTableColumnOptionOrWithColumnSyntax AsIntPK(this ICreateTableColumnAsTypeSyntax builder)
    {
        return builder.AsInt32().PrimaryKey().Identity();
    }
}
