using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Subject(typeof(Share.Where.ByTeacher<>))]
public class When_entity_has_teacher
{
    private static IQueryable<TeacherPreferences> entities;

    private static int teacherId;

    private Establish context = () =>
    {
        entities = Pleasure.ToQueryable(Pleasure.Generator.Invent<TeacherPreferences>(),
                                        Pleasure.Generator.Invent<TeacherPreferences>(),
                                        Pleasure.Generator.Invent<TeacherPreferences>());

        teacherId = entities.First().TeacherId;
    };

    private It should_filter_entities = () =>
    {
        var filtered = entities.Where(new Share.Where.ByTeacher<TeacherPreferences>(teacherId).IsSatisfiedBy())
                               .ToList();

        filtered.Count.ShouldEqual(1);
        filtered.First().TeacherId.ShouldEqual(teacherId);
    };
}
