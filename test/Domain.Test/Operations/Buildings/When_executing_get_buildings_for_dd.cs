using Domain.Api;
using Domain.Persistence;
using Incoding.Core.ViewModel;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetBuildingsForDDQuery), "Query")]
class When_executing_get_buildings_for_dd : query_spec<GetBuildingsForDDQuery, List<KeyValueVm>>
{
    static Building[] buildings;

    Establish context = () =>
    {
        query = new GetBuildingsForDDQuery();

        buildings = new[]
        {
                Pleasure.Generator.Invent<Building>(),
                Pleasure.Generator.Invent<Building>(),
                Pleasure.Generator.Invent<Building>()
        };

        mockQuery = MockQuery<GetBuildingsForDDQuery, List<KeyValueVm>>
                    .When(query)
                    .StubQuery(entities: buildings);
    };

    Because of = () => mockQuery.Execute();

    It should_return_list_of_key_value_vm = () =>
    {
        mockQuery.ShouldBeIsResult(list =>
        {
            list.ShouldEqualWeakEach(buildings,
                                     (dsl, i) =>
                                     {
                                         dsl.ForwardToValue(s => s.Value, buildings[i].Id.ToString());
                                         dsl.Forward(s => s.Text, b => b.Name);
                                     });
        });
    };
}
