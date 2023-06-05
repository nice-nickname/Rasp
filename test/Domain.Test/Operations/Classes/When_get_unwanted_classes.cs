using System.Drawing;
using Domain.Api;
using Domain.Persistence;
using Incoding.Core.Extensions;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetUnwantedClassesQuery), "Query")]
class When_get_unwanted_classes : query_spec<GetUnwantedClassesQuery, List<GetUnscheduledClassesQuery.Response>>
{
    static Func<Class> entityFactory = () => Pleasure.Generator.Invent<Class>(dsl => dsl.Tuning(s => s.IsUnwanted, true)
                                                                                        .GenerateTo(s => s.ScheduleFormat, sf => sf.Tuning(s => s.FacultyId, Pleasure.Generator.TheSameNumber()))
                                                                                        .GenerateTo(s => s.Plan,
                                                                                                    plan => plan.GenerateTo(s => s.Group)
                                                                                                                .GenerateTo(s => s.Teacher)
                                                                                                                .GenerateTo(s => s.SubDiscipline,
                                                                                                                            sd => sd.GenerateTo(s => s.Kind)
                                                                                                                                    .GenerateTo(s => s.Discipline, d => d.GenerateTo(s => s.Department)))));

    static Class[] classes;

    private Establish context = () =>
    {
        query = new GetUnwantedClassesQuery { FacultyId = Pleasure.Generator.TheSameNumber() };

        classes = new[]
        {
                entityFactory(),
                entityFactory(),
                entityFactory(),
                entityFactory()
        };
        mockQuery = MockQuery<GetUnwantedClassesQuery, List<GetUnscheduledClassesQuery.Response>>
                    .When(query)
                    .StubQuery(entities: classes);
    };

    Because of = () => mockQuery.Execute();

    It should_return_class_list = () => mockQuery.ShouldBeIsResult(list =>
    {
        list.ShouldEqualWeakEach(classes,
                                 (dsl, i) =>
                                 {
                                     dsl.ForwardToValue(s => s.TeacherId, classes[i].Plan.TeacherId)
                                        .Forward(s => s.Week, c => c.Week)
                                        .ForwardToValue(s => s.Group, classes[i].Plan.Group.Code)
                                        .Forward(s => s.SubGroupNo, c => c.SubGroupNo)
                                        .ForwardToValue(s => s.Teacher, classes[i].Plan.Teacher.Name)
                                        .ForwardToValue(s => s.TeacherShort, classes[i].Plan.Teacher.ShortName)
                                        .ForwardToValue(s => s.Discipline, classes[i].Plan.SubDiscipline.Discipline.Name)
                                        .ForwardToValue(s => s.DisciplineShort, classes[i].Plan.SubDiscipline.Discipline.Code)
                                        .ForwardToValue(s => s.SubDisciplineKind, classes[i].Plan.SubDiscipline.Kind.Name)
                                        .ForwardToValue(s => s.SubDisciplineKindShort, classes[i].Plan.SubDiscipline.Kind.Code)
                                        .ForwardToValue(s => s.Color, classes[i].Plan.SubDiscipline.Kind.Color.ToHex())
                                        .ForwardToValue(s => s.Department, classes[i].Plan.SubDiscipline.Discipline.Department?.Name ?? string.Empty)
                                        .ForwardToValue(s => s.DepartmentShort, classes[i].Plan.SubDiscipline.Discipline.Department?.Code ?? string.Empty)
                                        .ForwardToValue(s => s.HasSubGroups, classes[i].SubGroupNo > 1)
                                        .IgnoreBecauseNotUse(s => s.GroupId)
                                        ;
                                 });
    });
}
