using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetTeachersQuery), "Query")]
class When_executing_get_teachers : query_spec<GetTeachersQuery, List<GetTeachersQuery.Response>>
{
    static Teacher[] teachers;

    Establish context = () =>
    {
        query = new GetTeachersQuery();

        teachers = new[]
        {
                Pleasure.Generator.Invent<Teacher>(dsl => dsl.GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Teacher>(dsl => dsl.GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Teacher>(dsl => dsl.GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Teacher>(dsl => dsl.GenerateTo(s => s.Department))
        };

        mockQuery = MockQuery<GetTeachersQuery, List<GetTeachersQuery.Response>>
                    .When(query)
                    .StubQuery(entities: teachers);
    };

    Because of = () => mockQuery.Execute();

    It should_return_response_list_with_department = () =>
    {
        mockQuery.ShouldBeIsResult(list =>
        {
            list.ShouldEqualWeakEach(teachers.OrderBy(s => s.Name),
                                     (dsl, i) => dsl.ForwardToValue(s => s.DepartmentCode, teachers[i].Department.Code)
                                                    .ForwardToValue(s => s.DepartmentName, teachers[i].Department.Name)
                                                    .Forward(s => s.Initials, t => t.ShortName));
        });
    };
}
