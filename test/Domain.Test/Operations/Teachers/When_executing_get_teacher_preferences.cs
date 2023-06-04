using Domain.Api;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetTeacherPreferencesQuery), "Query")]
class When_executing_get_teacher_preferences : query_spec<GetTeacherPreferencesQuery, List<GetTeacherPreferencesQuery.Response>>
{
    private Establish context = () =>
    {
        query = Pleasure.Generator.Invent<GetTeacherPreferencesQuery>();

        schedule = new List<AddOrEditScheduleFormatCommand.ScheduleItem>
        {
                Pleasure.Generator.Invent<AddOrEditScheduleFormatCommand.ScheduleItem>(),
                Pleasure.Generator.Invent<AddOrEditScheduleFormatCommand.ScheduleItem>(),
                Pleasure.Generator.Invent<AddOrEditScheduleFormatCommand.ScheduleItem>(),
                Pleasure.Generator.Invent<AddOrEditScheduleFormatCommand.ScheduleItem>(),
                Pleasure.Generator.Invent<AddOrEditScheduleFormatCommand.ScheduleItem>(),
                Pleasure.Generator.Invent<AddOrEditScheduleFormatCommand.ScheduleItem>(),
        };

        teachers = new[]
        {
                new TeacherPreferences
                {
                        Day = DayOfWeek.Monday,
                        ScheduleFormatId = schedule[0].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Impossible
                },
                new TeacherPreferences
                {
                        Day = DayOfWeek.Tuesday,
                        ScheduleFormatId = schedule[1].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Impossible
                },
                new TeacherPreferences
                {
                        Day = DayOfWeek.Wednesday,
                        ScheduleFormatId = schedule[2].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Impossible
                },
                new TeacherPreferences
                {
                        Day = DayOfWeek.Thursday,
                        ScheduleFormatId = schedule[3].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Impossible
                },
                new TeacherPreferences
                {
                        Day = DayOfWeek.Friday,
                        ScheduleFormatId = schedule[4].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Impossible
                },
                new TeacherPreferences
                {
                        Day = DayOfWeek.Saturday,
                        ScheduleFormatId = schedule[5].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Impossible
                },


                new TeacherPreferences
                {
                        Day = DayOfWeek.Monday,
                        ScheduleFormatId = schedule[5].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Unwanted
                },
                new TeacherPreferences
                {
                        Day = DayOfWeek.Tuesday,
                        ScheduleFormatId = schedule[4].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Unwanted
                },
                new TeacherPreferences
                {
                        Day = DayOfWeek.Wednesday,
                        ScheduleFormatId = schedule[3].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Unwanted
                },
                new TeacherPreferences
                {
                        Day = DayOfWeek.Thursday,
                        ScheduleFormatId = schedule[2].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Unwanted
                },
                new TeacherPreferences
                {
                        Day = DayOfWeek.Friday,
                        ScheduleFormatId = schedule[1].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Unwanted
                },
                new TeacherPreferences
                {
                        Day = DayOfWeek.Saturday,
                        ScheduleFormatId = schedule[0].Id.GetValueOrDefault(),
                        Type = TeacherPreferences.PreferenceType.Unwanted
                },
        };

        mockQuery = MockQuery<GetTeacherPreferencesQuery, List<GetTeacherPreferencesQuery.Response>>
                    .When(query)
                    .StubQuery(new GetScheduleFormatQuery { FacultyId = query.FacultyId },
                               new GetScheduleFormatQuery.Response
                               {
                                       Items = schedule,
                                       ItemsCount = schedule.Count,
                               })
                    .StubQuery(whereSpecification: new Share.Where.ByTeacher<TeacherPreferences>(query.TeacherId.GetValueOrDefault()), 
                               entities: teachers);
    };

    Because of = () => mockQuery.Execute();

    private It should_return_preferences = () => mockQuery.ShouldBeIsResult(list =>
    {
        var mergedPreferences = list.SelectMany(s => s.Days).ToList();

        mergedPreferences.Count(s => s.Type == GetTeacherPreferencesQuery.PreferenceType.IMPOSSIBLE).ShouldEqual(6);
        mergedPreferences.Count(s => s.Type == GetTeacherPreferencesQuery.PreferenceType.UNWANTED).ShouldEqual(6);

        for (var i = 0; i < schedule.Count; i++)
        {
            list[i].Days[i].Type.ShouldEqual(GetTeacherPreferencesQuery.PreferenceType.IMPOSSIBLE);
            list[i].Days[schedule.Count - i - 1].Type.ShouldEqual(GetTeacherPreferencesQuery.PreferenceType.UNWANTED);
        }
    });

    static TeacherPreferences[] teachers;
    
    static List<AddOrEditScheduleFormatCommand.ScheduleItem> schedule;
}
