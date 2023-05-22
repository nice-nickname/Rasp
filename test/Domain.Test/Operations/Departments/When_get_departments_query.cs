using Domain.Api;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Subject(typeof(GetDepartmentsQuery))]
public class When_get_departments_query
{
    private Establish context = () =>
    {
        query = Pleasure.Generator.Invent<GetDepartmentsQuery>();

        var entities = new[]
        {
                Pleasure.Generator.Invent<Department>(),
                Pleasure.Generator.Invent<Department>(),
                Pleasure.Generator.Invent<Department>(dsl => dsl.Tuning(c => c.FacultyId, query.FacultyId)),
        };

        expected = entities.Where(s => s.FacultyId == query.FacultyId).ToArray().ToArray();

        mockQuery = MockQuery<GetDepartmentsQuery, List<GetDepartmentsQuery.Response>>
                        .When(query)
                        .StubQuery(whereSpecification: new Share.Where.ByFaculty<Department>(query.FacultyId), 
                                   entities: expected);

    };

    private Because of = () => mockQuery.Execute();

    private It should_be_filtered_by_faculty = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(expected.Where(s => s.FacultyId == query.FacultyId).ToArray()));

    private static MockMessage<GetDepartmentsQuery, List<GetDepartmentsQuery.Response>> mockQuery;

    private static GetDepartmentsQuery query;

    private static Department[] expected;
}
