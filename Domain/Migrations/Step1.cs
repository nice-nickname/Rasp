using FluentMigrator;

namespace Domain.Migrations
{
	[Migration(1, "First migration")]
	public class Step1 : Migration
	{
		public override void Up()
        {
            Create.Table("Building")
                  .WithColumn("Code").AsFixedLengthString(1).PrimaryKey();

            Create.Table("Auditorium")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("Building").AsFixedLengthString(1)
                  .WithColumn("Code").AsFixedLengthString(3);

            Create.ForeignKey("FK_Building_Auditoriums")
                  .FromTable("Auditorium")
                  .ForeignColumn("Building")
                  .ToTable("Building")
                  .PrimaryColumn("Code");
        }

		public override void Down()
        {
            Delete.Table("Auditorium");
            Delete.Table("Building");
        }
	}
}