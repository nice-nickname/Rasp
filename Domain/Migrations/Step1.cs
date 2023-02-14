using FluentMigrator;

namespace Domain.Migrations
{
	[Migration(1, "First migration")]
	public class Step1 : Migration
	{
		public override void Up()
		{
			Execute.Sql("SELECT 1");
		}

		public override void Down()
		{
			
		}

	}
}