using Domain.Persistence.Specification;
using Incoding.Core.Extensions.LinqSpecs;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.Where.ByTeacher<>), "Specification")]
class When_querying_entity_that_has_teacher
{
    Establish context = () =>
    {
        Func<int, Moq.Mock<Share.IEntityHasTeacher>> entity = id =>
        {
            var mock = new Moq.Mock<Share.IEntityHasTeacher>();
            mock.SetupProperty(s => s.TeacherId, id);
            return mock;
        };

        query = new List<Share.IEntityHasTeacher>
        {
            entity(1).Object,
            entity(2).Object,
            entity(3).Object,
            entity(1).Object
        }.AsQueryable();

        specification = new Share.Where.ByTeacher<Share.IEntityHasTeacher>(1);
    };

    Because of = () => query = query.Where(specification.IsSatisfiedBy());

    It should_be_filtered = () => query.Count().ShouldEqual(2);

    static IQueryable<Share.IEntityHasTeacher> query;

    static Specification<Share.IEntityHasTeacher> specification;
}
