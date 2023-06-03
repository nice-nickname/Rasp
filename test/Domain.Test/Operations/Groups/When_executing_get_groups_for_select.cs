using Domain.Api;
using Domain.Common;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;
using Resources;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetGroupsForSelectQuery), "Query")]
class When_executing_get_groups_for_select : query_spec<GetGroupsForSelectQuery, List<DropDownItem>>
{
    static Group[] groups;

    Establish context = () =>
    {
        query = new GetGroupsForSelectQuery { FacultyId = Pleasure.Generator.TheSameNumber() };

        groups = new[]
        {
                Pleasure.Generator.Invent<Group>(dsl => dsl.GenerateTo(s => s.Department, inDsl => inDsl.Tuning(s => s.FacultyId, query.FacultyId))),
                Pleasure.Generator.Invent<Group>(dsl => dsl.GenerateTo(s => s.Department, inDsl => inDsl.Tuning(s => s.FacultyId, query.FacultyId))),
                Pleasure.Generator.Invent<Group>(dsl => dsl.GenerateTo(s => s.Department, inDsl => inDsl.Tuning(s => s.FacultyId, query.FacultyId))),
                Pleasure.Generator.Invent<Group>(dsl => dsl.GenerateTo(s => s.Department, inDsl => inDsl.Tuning(s => s.FacultyId, query.FacultyId)))
        };

        mock = query => mockQuery = MockQuery<GetGroupsForSelectQuery, List<DropDownItem>>
                                    .When(query)
                                    .StubQuery(whereSpecification: new Share.Where.ByFacultyThoughDepartment<Group>(query.FacultyId), entities: groups);
    };

    class when_has_selected
    {
        Establish context = () =>
        {
            query.SelectedIds = new[] { groups[0].Id, groups[3].Id };

            mock(query);
        };

        Because of = () => mockQuery.Execute();

        It should_have_selected_items = () =>
        {
            mockQuery.ShouldBeIsResult(list =>
            {
                list[0].Selected.ShouldBeTrue();
                list[1].Selected.ShouldBeFalse();
                list[2].Selected.ShouldBeFalse();
                list[3].Selected.ShouldBeTrue();
            });
        };
    }

    class when_executed
    {
        Establish context = () => mock(query);

        Because of = () => mockQuery.Execute();

        It should_return_drop_down_item_list = () =>
        {
            mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(groups,
                                                                        (dsl, i) =>
                                                                        {
                                                                            dsl.ForwardToValue(s => s.Value, groups[i].Id);
                                                                            dsl.ForwardToValue(s => s.Text, groups[i].Code);
                                                                            dsl.ForwardToValue(s => s.SubText, groups[i].Department.Code);
                                                                            dsl.ForwardToValue(s => s.Group, $"{groups[i].Course} {DataResources.Course}");
                                                                            dsl.ForwardToValue(s => s.Selected, false);
                                                                            dsl.IgnoreBecauseCalculate(s => s.Search);
                                                                        }));
        };
    }
}
