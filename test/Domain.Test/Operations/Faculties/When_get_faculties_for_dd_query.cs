using Domain.Api;
using Domain.Persistence;
using Incoding.Core.ViewModel;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Subject(typeof(GetFacultyForDDQuery))]
public class When_get_faculties_for_dd_query
{
    private Establish context = () =>
    {
        query = Pleasure.Generator.Invent<GetFacultyForDDQuery>();

        mockFaculties = Pleasure.Generator.Invent<List<Faculty>>();

        mockQuery = MockQuery<GetFacultyForDDQuery, List<KeyValueVm>>
                    .When(query)
                    .StubQuery(entities: mockFaculties.ToArray());
    };

    private Because of = () => mockQuery.Execute();

    private It should_have_result = () =>
    {
        mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(mockFaculties,
                                                                    (dsl, i) => dsl.ForwardToValue(s => s.Text, $"{mockFaculties[i].Code} - {mockFaculties[i].Name}")
                                                                                   .ForwardToValue(s => s.Value, mockFaculties[i].Id.ToString())
                                                                                   .IgnoreBecauseNotUse(s => s.Selected)
                                                                                   .IgnoreBecauseNotUse(s => s.CssClass)
                                                                                   .IgnoreBecauseNotUse(s => s.Title)));
    };

    private It should_have_not_have_selected = () =>
    {
        mockQuery.ShouldBeIsResult(list => list.ShouldNotContain(l => l.Selected == true));
    };

    private static List<Faculty> mockFaculties;

    private static MockMessage<GetFacultyForDDQuery, List<KeyValueVm>> mockQuery;

    private static GetFacultyForDDQuery query;
}
