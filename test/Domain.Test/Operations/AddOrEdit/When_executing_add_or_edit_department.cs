using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(AddOrEditDepartmentCommand), "Command")]
class When_executing_add_or_edit_department
{
    static MockCommand<AddOrEditDepartmentCommand> mockCommand;

    static AddOrEditDepartmentCommand command;

    Establish context = () =>
    {
        command = Pleasure.Generator.Invent<AddOrEditDepartmentCommand>();
        mockCommand = MockCommand<AddOrEditDepartmentCommand>.When(command);
    };

    Because of = () => mockCommand.Execute();

    It should_save_or_update = () => mockCommand.ShouldBeSaveOrUpdate<Department>(d =>
    {
        d.Code.ShouldEqual(command.Code);
        d.Name.ShouldEqual(command.Name);
        d.FacultyId.ShouldEqual(command.FacultyId);
    });
}
