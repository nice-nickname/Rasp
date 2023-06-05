using System.Drawing;
using Domain.Api;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetSubDisciplineKindsQuery), "Query")]
class When_execution_get_subdiscipline_kinds : query_spec<GetSubDisciplineKindsQuery, List<GetSubDisciplineKindsQuery.Response>>
{
    private Establish context = () =>
    {
        query = new GetSubDisciplineKindsQuery();

        expected = new[]
        {
                Pleasure.Generator.Invent<SubDisciplineKind>(),
                Pleasure.Generator.Invent<SubDisciplineKind>(),
                Pleasure.Generator.Invent<SubDisciplineKind>(),
                Pleasure.Generator.Invent<SubDisciplineKind>()
        };

        mockQuery = MockQuery<GetSubDisciplineKindsQuery, List<GetSubDisciplineKindsQuery.Response>>
                    .When(query)
                    .StubQuery(entities: expected);
    };

    Because of = () => mockQuery.Execute();

    private It should_return_response_list = () => mockQuery.ShouldBeIsResult(list =>
    {
        list.ShouldEqualWeakEach(expected,
                                 (dsl, i) =>
                                 {
                                     dsl.ForwardToValue(s => s.HtmlColor, ColorTranslator.ToHtml(expected[i].Color));
                                 });
    });

    static SubDisciplineKind[] expected;
}

