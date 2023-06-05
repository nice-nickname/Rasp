using Domain.Api;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.Extensions;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(MarkTeacherPreferencesCommand), "Command")]
class When_executing_mark_teacher_preferences
{
    static MarkTeacherPreferencesCommand command;

    private Establish context = () =>
    {
        command = Pleasure.Generator.Invent<MarkTeacherPreferencesCommand>();

        preference = new TeacherPreferences
        {
                Day = command.Day,
                ScheduleFormatId = command.ScheduleFormatId,
                TeacherId = command.TeacherId,
                Id = Pleasure.Generator.PositiveNumber(),
                Type = TeacherPreferences.PreferenceType.Unwanted
        };

        classes = new[]
        {
                new Class
                {
                        ScheduleFormatId = command.ScheduleFormatId,
                        Plan = new DisciplinePlan { TeacherId = command.TeacherId }
                },
                new Class
                {
                        ScheduleFormatId = command.ScheduleFormatId,
                        Plan = new DisciplinePlan { TeacherId = command.TeacherId }
                },
                new Class
                {
                        ScheduleFormatId = command.ScheduleFormatId,
                        Plan = new DisciplinePlan { TeacherId = command.TeacherId }
                },
                new Class
                {
                        ScheduleFormatId = Pleasure.Generator.PositiveNumber(),
                        Plan = new DisciplinePlan { TeacherId = command.TeacherId }
                }
        };
    };

    class when_preference_is_none
    {
        Establish context = () =>
        {
            command.Type = GetTeacherPreferencesQuery.PreferenceType.NONE;

            mockCommand = MockCommand<MarkTeacherPreferencesCommand>
                              .When(command)
                              .StubQuery(whereSpecification: new Share.Where.ByTeacher<TeacherPreferences>(command.TeacherId)
                                                 .And(new TeacherPreferences.Where.ByDay(command.Day, command.ScheduleFormatId)),
                                         entities: new[] { preference });
        };

        Because of = () => mockCommand.Execute();

        It should_delete_existing_preference = () => mockCommand.ShouldBeDelete<TeacherPreferences>(preference.Id);
    }

    class when_preference_is_unwanted_or_impossible
    {
        Establish context = () =>
        {
            command.Type = GetTeacherPreferencesQuery.PreferenceType.UNWANTED;

            mockCommand = MockCommand<MarkTeacherPreferencesCommand>
                          .When(command)
                          .StubQuery(whereSpecification: new Share.Where.ByTeacher<TeacherPreferences>(command.TeacherId)
                                             .And(new TeacherPreferences.Where.ByDay(command.Day, command.ScheduleFormatId)),
                                     entities: new[] { preference })
                          .StubQuery(entities: classes);
        };

        Because of = () => mockCommand.Execute();

        It should_save_preference = () => mockCommand.ShouldBeSaveOrUpdate<TeacherPreferences>(saved =>
        {
            saved.Type.ShouldEqual(command.Type switch
            {
                    GetTeacherPreferencesQuery.PreferenceType.IMPOSSIBLE => TeacherPreferences.PreferenceType.Impossible,
                    GetTeacherPreferencesQuery.PreferenceType.UNWANTED => TeacherPreferences.PreferenceType.Unwanted,
                    _ => throw new ArgumentOutOfRangeException()
            });
            saved.Day = command.Day;
            saved.ScheduleFormatId = command.ScheduleFormatId;
            saved.TeacherId = command.TeacherId;
        });

        It should_set_unwanted_to_related_classes = () =>
        {
            classes.Where(s => s.ScheduleFormatId == command.ScheduleFormatId && s.Plan.TeacherId == command.TeacherId)
                   .ShouldEachConformTo(c => c.IsUnwanted);
        };
    }

    static TeacherPreferences preference;

    static Class[] classes;

    static MockMessage<MarkTeacherPreferencesCommand, object> mockCommand;
}
