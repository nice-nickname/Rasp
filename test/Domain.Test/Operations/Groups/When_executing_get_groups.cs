using Domain.Api;
using Domain.Persistence;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetGroupsQuery), "Query")]
class When_executing_get_groups : query_spec<GetGroupsQuery, List<GetGroupsQuery.Response>>
{
    static Group[] groups;

    Establish context = () =>
    {
        query = new GetGroupsQuery();

        groups = new[]
        {
                Pleasure.Generator.Invent<Group>(dsl => dsl.GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Group>(dsl => dsl.GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Group>(dsl => dsl.GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Group>(dsl => dsl.GenerateTo(s => s.Department))
        };

        mockQuery = MockQuery<GetGroupsQuery, List<GetGroupsQuery.Response>>
                    .When(query)
                    .StubQuery(entities: groups);
    };

    Because of = () => mockQuery.Execute();

    It should_return_list_of_groups = () =>
    {
        mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(groups.OrderBy(s => s.Course).ThenBy(s => s.Code),
                                                                    (dsl, i) =>
                                                                    {
                                                                        dsl.Forward(s => s.Id, g => g.Id);
                                                                        dsl.Forward(s => s.Code, g => g.Code);
                                                                        dsl.Forward(s => s.Course, g => g.Course);
                                                                        dsl.Forward(s => s.DepartmentId, g => g.DepartmentId);
                                                                        dsl.ForwardToValue(s => s.DepartmentName, groups[i].Department.Name);
                                                                        dsl.ForwardToValue(s => s.DepartmentCode, groups[i].Department.Code);
                                                                    }));
    };
}
