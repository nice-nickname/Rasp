using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(AddOrEditAuditoriumCommand), "Command")]
class When_executing_add_or_edit_auditorium : command_spec<AddOrEditAuditoriumCommand>
{
    Establish context = () =>
    {
        command = Pleasure.Generator.Invent<AddOrEditAuditoriumCommand>();
        command.Kinds = new List<AddOrEditAuditoriumCommand.TempAuditoriumKind>
        {
                Pleasure.Generator.Invent<AddOrEditAuditoriumCommand.TempAuditoriumKind>(),
                Pleasure.Generator.Invent<AddOrEditAuditoriumCommand.TempAuditoriumKind>(),
                Pleasure.Generator.Invent<AddOrEditAuditoriumCommand.TempAuditoriumKind>()
        };
        mockCommand = MockCommand<AddOrEditAuditoriumCommand>
                      .When(command)
                      .StubGetById(command.Id, Pleasure.Generator.Invent<Auditorium>());
    };

    Because of = () => mockCommand.Execute();

    It should_save_or_update = () => mockCommand.ShouldBeSaveOrUpdate<Auditorium>(d =>
    {
        d.Code.ShouldEqual(command.Code);
        d.BuildingId.ShouldEqual(command.BuildingId);
        d.DepartmentId.ShouldEqual(command.DepartmentId);
        d.Capacity.ShouldEqual(command.Capacity);
        d.Kinds.ShouldEqual(command.Kinds.Where(s => s.IsSelected).Select(s => new AuditoriumKind
        {
                Kind = s.Kind,
                Id = s.Id,
        }));
    });
}
