using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(AddOrEditGroupCommand), "Command")]
class When_executing_add_or_edit_group : command_spec<AddOrEditGroupCommand>
{
    Establish context = () =>
    {
        command = Pleasure.Generator.Invent<AddOrEditGroupCommand>();
        mockCommand = MockCommand<AddOrEditGroupCommand>
                        .When(command)
                        .StubGetById(command.Id, Pleasure.Generator.Invent<Group>());
    };

    Because of = () => mockCommand.Execute();

    It should_save_or_update = () => mockCommand.ShouldBeSaveOrUpdate<Group>(d =>
    {
        d.Code.ShouldEqual(command.Code);
        d.Course.ShouldEqual(command.Course);
        d.DepartmentId.ShouldEqual(command.DepartmentId);
        d.StudentCount.ShouldEqual(command.StudentCount);
    });
}
