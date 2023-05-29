using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(AddOrEditDepartmentCommand), "Command")]
public class When_executeing_add_or_edit_department
{
    private Establish context = () =>
    {
        command = Pleasure.Generator.Invent<AddOrEditDepartmentCommand>();
        mockCommand = MockCommand<AddOrEditDepartmentCommand>.When(command);
    };

    private Because of = () => mockCommand.Execute();

    private It should_save_or_update = () => mockCommand.ShouldBeSaveOrUpdate<Department>(d =>
    {
        d.Code.ShouldEqual(command.Code);
        d.Name.ShouldEqual(command.Name);
        d.FacultyId.ShouldEqual(command.FacultyId);
    });

    private static MockCommand<AddOrEditDepartmentCommand> mockCommand;

    private static AddOrEditDepartmentCommand command;
}

