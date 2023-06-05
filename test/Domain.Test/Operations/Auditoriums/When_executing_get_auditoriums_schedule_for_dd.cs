using Domain.Api;
using Domain.Persistence;
using Incoding.Core.ViewModel;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetAuditoriumsScheduleForDDQuery), "Query")]
class When_executing_get_auditoriums_schedule_for_dd : query_spec<GetAuditoriumsScheduleForDDQuery, List<KeyValueVm>>
{
    static GetAuditoriumsScheduleForDDQuery query;

    static MockMessage<GetAuditoriumsScheduleForDDQuery, List<KeyValueVm>> mockQuery;

    static Auditorium[] auditoriums;

    static AuditoriumToKinds[] kinds;

    static Group group;

    static SubDiscipline subDiscipline;

    private Establish context = () =>
    {
        query = Pleasure.Generator.Invent<GetAuditoriumsScheduleForDDQuery>();

        subDiscipline = new SubDiscipline
        {
                Id = query.SubDisciplineId,
                Discipline = new Discipline
                {
                        Groups = new[]
                        {
                                new Group { Id = query.GroupId.GetValueOrDefault(), StudentCount = 20 },
                                new Group { StudentCount = 20 },
                                new Group { StudentCount = 20 }
                        }
                },
                IsParallelHours = false
        };

        group = subDiscipline.Discipline.Groups[0];

        auditoriums = new[]
        {
                Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)
                                                                .Tuning(s => s.Capacity, 19)),
                Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)
                                                                .Tuning(s => s.Capacity, 20)),
                Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)
                                                                .Tuning(s => s.Capacity, 22)),
                Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)
                                                                .Tuning(s => s.Capacity, 59)),
                Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)
                                                                .Tuning(s => s.Capacity, 60)),
                Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)
                                                                .Tuning(s => s.Capacity, 80))
        };

        kinds = Array.Empty<AuditoriumToKinds>();

        var @class = Pleasure.Generator.Invent<Class>(dsl => dsl.Tuning(s => s.Day, query.Day)
                                                                .Tuning(s => s.Week, query.Week)
                                                                .Tuning(s => s.ScheduleFormatId, query.ScheduleFormatId)
                                                                .Tuning(s => s.AuditoriumId, auditoriums[5].Id));

        mock = q =>
        {
            mockQuery = MockQuery<GetAuditoriumsScheduleForDDQuery, List<KeyValueVm>>
                        .When(q)
                        .StubGetById(query.SubDisciplineId, subDiscipline)
                        .StubQuery(entities: kinds)
                        .StubQuery(entities: new[] { @class })
                        .StubQuery(entities: auditoriums);

            if (!subDiscipline.IsParallelHours)
            {
                mockQuery = mockQuery.StubGetById(query.GroupId, group);
            }
        };
    };

    class when_sub_discipline_is_parallel
    {
        Establish context = () =>
        {
            subDiscipline.IsParallelHours = true;
            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_return_auditoriums_fits_all_groups = () =>
        {
            var studentsInGroups = subDiscipline.Discipline.Groups.Sum(s => s.StudentCount);
            mockQuery.ShouldBeIsResult(list => list.ShouldContain(s => auditoriums.First(c => c.Id.ToString() == s.Value).Capacity >= studentsInGroups));
        };
    }

    class when_sub_discipline_is_not_parallel
    {
        Establish context = () => mock(query);

        Because of = () => mockQuery.Execute();

        It should_return_auditoriums_fit_exact_group = () =>
        {
            mockQuery.ShouldBeIsResult(list => list.ShouldContain(s => auditoriums.First(c => c.Id.ToString() == s.Value).Capacity >= group.StudentCount));
        };
    }

    class when_has_preferences_by_auditoriums
    {
        Establish context = () =>
        {
            subDiscipline.Auditoriums = new List<Auditorium> { auditoriums[0], auditoriums[1] };
            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_return_preferred_auditorium_and_fit_capacity = () =>
        {
            mockQuery.ShouldBeIsResult(list =>
            {
                list.Count.ShouldEqual(1);
                list[0].Value.ShouldEqual(auditoriums[1].Id.ToString());
            });
        };
    }

    class when_has_preferences_by_kinds
    {
        Establish context = () =>
        {
            subDiscipline.AuditoriumKinds = new List<AuditoriumKind>
            {
                    new() { Id = 1 },
                    new() { Id = 2 }
            };

            kinds = new[]
            {
                    new AuditoriumToKinds { AuditoriumKindId = 1, AuditoriumId = auditoriums[0].Id },
                    new AuditoriumToKinds { AuditoriumKindId = 2, AuditoriumId = auditoriums[1].Id }
            };

            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_return_preferred_auditorium_and_fit_capacity = () =>
        {
            mockQuery.ShouldBeIsResult(list =>
            {
                list.Count.ShouldEqual(1);
                list[0].Value.ShouldEqual(auditoriums[1].Id.ToString());
            });
        };
    }

    class when_executed
    {
        Establish context = () => mock(query);

        Because of = () => mockQuery.Execute();

        It should_have_disabled_busy_auditorium = () =>
        {
            mockQuery.ShouldBeIsResult(list => list[4].CssClass.ShouldEqual("disabled"));
        };

        private It should_return_auditoriums_with_buildings = () =>
        {
            mockQuery.ShouldBeIsResult(list =>
            {
                list.ShouldEqualWeakEach(auditoriums.Where(s => s.Capacity >= 20),
                                         (dsl, i) =>
                                         {
                                             var auditorium = auditoriums.Where(s => s.Capacity >= 20).ToList()[i];
                                             dsl.ForwardToValue(s => s.Value, auditorium.Id.ToString())
                                                .ForwardToValue(s => s.Text, $"{auditorium.Building.Name}-{auditorium.Code}")
                                                .IgnoreBecauseRoot(s => s.CssClass);
                                         });
            });
        };
    }
}
