using Domain.Api;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.ViewModel;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Subject(typeof(GetDepartmentsForDDQuery))]
public class When_get_departments_for_dd_query_with_selected
{
    private Establish context = () =>
    {
        query = Pleasure.Generator.Invent<GetDepartmentsForDDQuery>(dsl => dsl.Tuning(s => s.Optional, null)
                                                                              .Tuning(s => s.SelectedId, Pleasure.Generator.TheSameNumber()));

        expected = new[]
        {
                Pleasure.Generator.Invent<Department>(dsl => dsl.Tuning(s => s.FacultyId, query.FacultyId)),
                Pleasure.Generator.Invent<Department>(dsl => dsl.Tuning(s => s.FacultyId, query.FacultyId)),
                Pleasure.Generator.Invent<Department>(dsl => dsl.Tuning(s => s.FacultyId, query.FacultyId)
                                                                .Tuning(s => s.Id, query.SelectedId))
        };

        mockQuery = MockQuery<GetDepartmentsForDDQuery, List<KeyValueVm>>
                    .When(query)
                    .StubQuery(whereSpecification: new Share.Where.ByFaculty<Department>(query.FacultyId),
                               entities: expected);
    };

    private Because of = () => mockQuery.Execute();

    private It should_have_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(expected,
                                                                                                      (dsl, i) => dsl.ForwardToValue(s => s.Value, expected[i].Id.ToString())
                                                                                                                     .Forward(s => s.Text, d => d.Code)));

    private It should_have_selected = () => mockQuery.ShouldBeIsResult(list => list.ShouldNotContain(l => l.Selected == true && l.Value != query.SelectedId.ToString()));

    private static Department[] expected;

    private static GetDepartmentsForDDQuery query;

    private static MockMessage<GetDepartmentsForDDQuery, List<KeyValueVm>> mockQuery;
}
