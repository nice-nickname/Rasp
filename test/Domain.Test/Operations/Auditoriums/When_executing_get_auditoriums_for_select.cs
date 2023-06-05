using Domain.Api;
using Domain.Common;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetAuditoriumsForSelectQuery), "Query")]
class When_executing_get_auditoriums_for_select
{
    private static Action<GetAuditoriumsForSelectQuery> mock;

    private static MockMessage<GetAuditoriumsForSelectQuery, List<DropDownItem>> mockQuery;

    static GetAuditoriumsForSelectQuery query;

    static Auditorium[] expected;

    Establish context = () =>
    {
        query = new GetAuditoriumsForSelectQuery();
        expected = new[]
        {
                Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)
                                                                .GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)
                                                                .GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)
                                                                .GenerateTo(s => s.Department)),
        };

        mock = query => mockQuery = MockQuery<GetAuditoriumsForSelectQuery, List<DropDownItem>>
                                    .When(query)
                                    .StubQuery(entities: expected);
    };

    class with_selected
    {
        Establish context = () =>
        {
            query.SelectedIds = new List<int> { expected[0].Id, expected[1].Id };
            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_have_selected = () =>
        {
            mockQuery.ShouldBeIsResult(list =>
            {
                list[0].Selected.ShouldBeTrue();
                list[1].Selected.ShouldBeTrue();
            });
        };
    }

    class when_executed
    {
        Establish context = () => mock(query);

        Because of = () => mockQuery.Execute();

        It should_return_drop_down_items = () =>
        {
            mockQuery.ShouldBeIsResult(list =>
            {
                list.ShouldEqualWeakEach(expected,
                                         (dsl, i) =>
                                         {
                                             var name = $"{expected[i].Building.Name}-{expected[i].Code}";
                                             dsl.ForwardToValue(s => s.Value, expected[i].Id as object)
                                                .ForwardToValue(s => s.Text, name)
                                                .ForwardToValue(s => s.SubText, expected[i].Department.Code)
                                                .ForwardToValue(s => s.Group, expected[i].Building.Name)
                                                .ForwardToValue(s => s.Search, $"{expected[i].Building.Name} {expected[i].Department.Code} {name} ")
                                                .ForwardToValue(s => s.Selected, false);
                                         });
            });
        };
    }
}
