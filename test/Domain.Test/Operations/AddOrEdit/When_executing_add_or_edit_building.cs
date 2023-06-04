using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(AddOrEditBuildingCommand), "Command")]
class When_executing_add_or_edit_building : command_spec<AddOrEditBuildingCommand>
{
    Establish context = () =>
    {
        command = Pleasure.Generator.Invent<AddOrEditBuildingCommand>();
        mockCommand = MockCommand<AddOrEditBuildingCommand>
                      .When(command)
                      .StubGetById(command.Id, Pleasure.Generator.Invent<Building>());
    };

    Because of = () => mockCommand.Execute();

    It should_save_or_update = () => mockCommand.ShouldBeSaveOrUpdate<Building>(d =>
    {
        d.Name.ShouldEqual(command.Name);
    });
}
