using Domain.Api;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.ViewModel;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;
using NHibernate.Hql.Ast.ANTLR.Tree;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetAuditoriumsForDDQuery), "Query")]
class When_executing_get_auditoriums_for_dd
{
    static Auditorium[] expected;
    
    static GetAuditoriumsForDDQuery query;
    
    static MockMessage<GetAuditoriumsForDDQuery, List<KeyValueVm>> mockQuery;
    
    Establish context = () =>
    {
        query = Pleasure.Generator.Invent<GetAuditoriumsForDDQuery>();

        expected = new[]
        {
            Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)),
            Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building)),
            Pleasure.Generator.Invent<Auditorium>(dsl => dsl.GenerateTo(s => s.Building))
        };

        mockQuery = MockQuery<GetAuditoriumsForDDQuery, List<KeyValueVm>>
                        .When(query)
                        .StubQuery(entities: expected);
    };

    Because of = () => mockQuery.Execute();

    It should_return_list_of_departments_with_building = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(expected, (dsl, i) =>
    {
        dsl.ForwardToValue(s => s.Value, expected[i].Id.ToString())
           .ForwardToValue(s => s.Text, expected[i].Building.Name + "-" + expected[i].Code);
    }));
}
