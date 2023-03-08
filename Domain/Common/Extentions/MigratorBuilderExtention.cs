using FluentMigrator.Builders.Create.Table;

namespace Domain.Extentions;

public static partial class Extention
{
	public static ICreateTableColumnOptionOrWithColumnSyntax AsIntPK(this ICreateTableColumnAsTypeSyntax builder)
	{
		return builder.AsInt32().PrimaryKey().Identity();
	}
}