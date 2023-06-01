using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetBuildingsQuery), "Query")]
class When_executing_get_buildings : query_spec<GetBuildingsQuery, List<GetBuildingsQuery.Response>>
{
    Establish context = () => 
    {
        query = new GetBuildingsQuery();

        expected = Pleasure.Generator.Invent<Building[]>();

        mockQuery = MockQuery<GetBuildingsQuery, List<GetBuildingsQuery.Response>>
                        .When(query)
                        .StubQuery(entities: expected);
    };

    Because of = () => mockQuery.Execute();

    It should_return_buildings_response = () => mockQuery.ShouldBeIsResult(list =>
    {
        list.ShouldEqualWeakEach(expected.OrderBy(s => s.Name));
    });

    static Building[] expected;
}
