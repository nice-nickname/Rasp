using Domain.Api;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetDepartmentsQuery), "Query")]
class When_executing_get_departments
{
    Establish context = () =>
    {
        query = Pleasure.Generator.Invent<GetDepartmentsQuery>();

        var departments = Pleasure.Generator.Invent<List<Department>>().ToArray();

        mockQuery = MockQuery<GetDepartmentsQuery, List<GetDepartmentsQuery.Response>>
                        .When(query)
                        .StubQuery(whereSpecification: new Share.Where.ByFaculty<Department>(query.FacultyId), 
                                   entities: departments);

        expected = departments.OrderBy(s => s.Code).ToArray();
    };

    Because of = () => mockQuery.Execute();

    It should_return_list_of_departments = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(expected));

    private static MockMessage<GetDepartmentsQuery, List<GetDepartmentsQuery.Response>> mockQuery;

    private static GetDepartmentsQuery query;

    private static Department[] expected;
}
