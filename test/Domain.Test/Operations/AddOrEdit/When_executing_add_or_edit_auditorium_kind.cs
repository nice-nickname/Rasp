using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(AddOrEditTeacherCommand), "Command")]
class When_executing_add_or_edit_auditorium_kind: command_spec<AddOrEditAuditoriumKindCommand>
{
    private Establish context = () =>
    {
        auditoriumKind = Pleasure.Generator.Invent<AuditoriumKind>();

        command = Pleasure.Generator.Invent<AddOrEditAuditoriumKindCommand>(dsl => dsl.Tuning(s => s.Id, auditoriumKind.Id));

        mockCommand = MockCommand<AddOrEditAuditoriumKindCommand>
                      .When(command)
                      .StubGetById(command.Id.GetValueOrDefault(), auditoriumKind);
    };

    Because of = () => mockCommand.Execute();

    It should_save_or_update = () => mockCommand.ShouldBeSaveOrUpdate<AuditoriumKind>(saved =>
    {
        saved.Kind = command.Kind;
    });

    static AuditoriumKind auditoriumKind;
}
