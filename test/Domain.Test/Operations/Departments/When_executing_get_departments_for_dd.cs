using Domain.Api;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.ViewModel;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetDepartmentsForDDQuery), "Query")]
class When_executing_get_departments_for_dd
{
    static MockMessage<GetDepartmentsForDDQuery, List<KeyValueVm>> mockQuery;

    static Action<GetDepartmentsForDDQuery> mock;

    static GetDepartmentsForDDQuery query;

    static Department[] expected;

    Establish context = () =>
    {
        mock = query => mockQuery = MockQuery<GetDepartmentsForDDQuery, List<KeyValueVm>>
                                    .When(query)
                                    .StubQuery(whereSpecification: new Share.Where.ByFaculty<Department>(query.FacultyId),
                                               entities: expected);

        query = new GetDepartmentsForDDQuery { FacultyId = 1 };

        expected = new[]
        {
                Pleasure.Generator.Invent<Department>(dsl => dsl.Tuning(s => s.FacultyId, query.FacultyId)),
                Pleasure.Generator.Invent<Department>(dsl => dsl.Tuning(s => s.FacultyId, query.FacultyId)),
                Pleasure.Generator.Invent<Department>(dsl => dsl.Tuning(s => s.FacultyId, query.FacultyId)),
                Pleasure.Generator.Invent<Department>(dsl => dsl.Tuning(s => s.FacultyId, query.FacultyId)
                                                                .Tuning(s => s.Id, Pleasure.Generator.TheSameNumber()))
        };

        mock(query);
    };

    class when_has_selected
    {
        Establish context = () =>
        {
            query.SelectedId = Pleasure.Generator.TheSameNumber();
            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_have_selected = () =>
        {
            mockQuery.ShouldBeIsResult(list => list.ShouldContain(s => s.Selected == true &&
                                                                       int.Parse(s.Value) == query.SelectedId));
        };
    }

    class when_has_optional
    {
        Establish context = () =>
        {
            query.Optional = Pleasure.Generator.TheSameString();
            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_have_optional_value = () => mockQuery.ShouldBeIsResult(list =>
        {
            list[0].Value.ShouldBeNull();
            list[0].Text.ShouldEqual(query.Optional);
        });
    }

    class when_executed
    {
        Because of = () => mockQuery.Execute();

        It should_return_list_of_key_value_vm = () => mockQuery.ShouldBeIsResult(list =>
        {
            list.ShouldEqualWeakEach(expected,
                                     (dsl, i) => dsl.ForwardToValue(s => s.Value, expected[i].Id.ToString())
                                                    .Forward(s => s.Text, d => d.Code)
                                                    .ForwardToValue(s => s.Selected, false));
        });
    }
}
