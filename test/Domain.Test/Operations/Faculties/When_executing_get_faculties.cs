using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetFacultiesQuery), "Query")]
class When_executing_get_faculties
{
    static Faculty[] expected;

    static MockMessage<GetFacultiesQuery, List<GetFacultiesQuery.Response>> mockQuery;

    Establish context = () =>
    {
        var query = Pleasure.Generator.Invent<GetFacultiesQuery>();

        var faculties = Pleasure.Generator.Invent<List<Faculty>>().ToArray();

        mockQuery = MockQuery<GetFacultiesQuery, List<GetFacultiesQuery.Response>>
                    .When(query)
                    .StubQuery(entities: faculties);

        expected = faculties.OrderBy(s => s.Code).ToArray();
    };

    Because of = () => mockQuery.Execute();

    It should_have_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(expected));
}
