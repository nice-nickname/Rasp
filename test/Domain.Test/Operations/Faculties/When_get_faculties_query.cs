using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Subject(typeof(GetFacultiesQuery))]
public class When_get_faculties_query
{
    private static MockMessage<GetFacultiesQuery, List<GetFacultiesQuery.Response>> mockQuery;

    private static List<GetFacultiesQuery.Response> faculties;

    private Establish context = () =>
    {
        var query = Pleasure.Generator.Invent<GetFacultiesQuery>();

        var mockFaculties = Pleasure.Generator.Invent<List<Faculty>>().ToArray();

        mockQuery = MockQuery<GetFacultiesQuery, List<GetFacultiesQuery.Response>>
                    .When(query)
                    .StubQuery(entities: mockFaculties);

        faculties = mockFaculties
                    .Select(s => new GetFacultiesQuery.Response
                    {
                            Name = s.Name,
                            Code = s.Code,
                            Id = s.Id,
                    })
                    .OrderBy(s => s.Code)
                    .ToList();
    };

    private Because of = () => mockQuery.Execute();

    private It should_be_ordered_by_code = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqual(faculties));

    private It should_contain_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldNotBeEmpty());
}
