using Domain.Api;
using Domain.Persistence;
using Incoding.Core.ViewModel;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetFacultyForDDQuery), "Query")]
class When_executing_get_faculties_for_dd
{
    Establish context = () =>
    {
        mock = query => mockQuery = MockQuery<GetFacultyForDDQuery, List<KeyValueVm>>
                                        .When(query)
                                        .StubQuery(entities: expected.ToArray());
        
        query = new GetFacultyForDDQuery();
        expected = Pleasure.Generator.Invent<List<Faculty>>();
    };

    class when_has_selected
    {
        Establish context = () => 
        {
            query.Selected = expected.First().Id;
            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_have_selected = () => mockQuery.ShouldBeIsResult(list => list[0].Selected.ShouldBeTrue());
    }

    class when_executed
    {
        Establish context = () => mock(query);

        Because of = () => mockQuery.Execute();

        It should_have_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(expected, 
                                                                  (dsl, i) => dsl.ForwardToValue(s => s.Value, expected[i].Id.ToString())
                                                                                 .ForwardToValue(s => s.Text, $"{expected[i].Code} - {expected[i].Name}")));
    }

    static MockMessage<GetFacultyForDDQuery, List<KeyValueVm>> mockQuery;

    static GetFacultyForDDQuery query;

    static List<Faculty> expected;

    static Action<GetFacultyForDDQuery> mock;
}
