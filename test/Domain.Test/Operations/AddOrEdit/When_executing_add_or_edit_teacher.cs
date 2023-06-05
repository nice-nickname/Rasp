using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(AddOrEditTeacherCommand), "Command")]
class When_executing_add_or_edit_teacher : command_spec<AddOrEditTeacherCommand>
{
    private Establish context = () =>
    {
        teacher = Pleasure.Generator.Invent<Teacher>();

        command = Pleasure.Generator.Invent<AddOrEditTeacherCommand>(dsl => dsl.Tuning(s => s.Id, teacher.Id));

        mockCommand = MockCommand<AddOrEditTeacherCommand>
                      .When(command)
                      .StubGetById(command.Id.GetValueOrDefault(), teacher);
    };

    Because of = () => mockCommand.Execute();
    
    It should_save_or_update = () => mockCommand.ShouldBeSaveOrUpdate<Teacher>(saved =>
    {
        saved.Name = command.Name;
        saved.DepartmentId = command.DepartmentId;
    });

    static Teacher teacher;
}
