using Domain.Api;
using Domain.Common;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetTeachersForSelectQuery), "Query")]
class When_executing_get_teachers_for_dd : query_spec<GetTeachersForSelectQuery, List<DropDownItem>>
{
    private static Teacher[] teachers;

    private Establish context = () =>
    {
        query = new GetTeachersForSelectQuery { FacultyId = Pleasure.Generator.PositiveNumber() };

        teachers = new[]
        {
                Pleasure.Generator.Invent<Teacher>(dsl => dsl.GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Teacher>(dsl => dsl.GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Teacher>(dsl => dsl.GenerateTo(s => s.Department)),
                Pleasure.Generator.Invent<Teacher>(dsl => dsl.GenerateTo(s => s.Department))
        };

        mock = q => mockQuery = MockQuery<GetTeachersForSelectQuery, List<DropDownItem>>
                                .When(q)
                                .StubQuery(whereSpecification: new Share.Where.ByFacultyThoughDepartment<Teacher>(query.FacultyId), 
                                           entities: teachers);
    };

    class when_executed
    {
        Establish context = () => mock(query);

        Because of = () => mockQuery.Execute();

        private It should_return_teachers_list = () => mockQuery.ShouldBeIsResult(list =>
        {
            list.ShouldEqualWeakEach(teachers,
                                     (dsl, i) =>
                                     {
                                         dsl.ForwardToValue(s => s.Value, teachers[i].Id)
                                            .ForwardToValue(s => s.Text, teachers[i].Name)
                                            .ForwardToValue(s => s.Selected, false)
                                            .ForwardToValue(s => s.SubText, "")
                                            .ForwardToValue(s => s.Group, teachers[i].Department.Code)
                                            .IgnoreBecauseCalculate(s => s.Search);
                                     });
        });
    }
}
