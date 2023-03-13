using Domain.Extentions;
using FluentMigrator;
using System.Data;

namespace Domain.Persistence;

[Migration(2, "Added tables for Class, ClassModule")]
public class Step2 : Migration
{
	public override void Up()
	{
		Create.Table(nameof(Class))
			  .WithColumn(nameof(Class.Id)).AsIntPK()
			  .WithColumn(nameof(Class.Name)).AsString(100);

		Create.Table(nameof(ClassModule))
			  .WithColumn(nameof(ClassModule.Id)).AsIntPK()
			  .WithColumn(nameof(ClassModule.ClassId)).AsInt32()
			  .WithColumn(nameof(ClassModule.TeacherId)).AsInt32()
			  .WithColumn(nameof(ClassModule.LectureHours)).AsInt16().WithDefaultValue(0)
			  .WithColumn(nameof(ClassModule.LaboratoryHours)).AsInt16().WithDefaultValue(0)
			  .WithColumn(nameof(ClassModule.PracticeHours)).AsInt16().WithDefaultValue(0);

		Create.ForeignKey("FK_ClassModule_Class")
			  .FromTable(nameof(ClassModule)).ForeignColumn(nameof(ClassModule.ClassId))
			  .ToTable(nameof(Class)).PrimaryColumn(nameof(Class.Id))
			  .OnDeleteOrUpdate(Rule.None);
		
		Create.ForeignKey("FK_ClassModule_Teacher")
			  .FromTable(nameof(ClassModule)).ForeignColumn(nameof(ClassModule.TeacherId))
			  .ToTable(nameof(Teacher)).PrimaryColumn(nameof(Teacher.Id))
			  .OnDeleteOrUpdate(Rule.None);
	}

	public override void Down()
	{
		Delete.Table(nameof(ClassModule));
		Delete.Table(nameof(Class));
	}
}