using Domain.Persistence.Specification;
using Incoding.Core.Extensions.LinqSpecs;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.IEntityHasTeacher), "Specification")]
class When_querying_entity_that_has_teacher
{
    static IQueryable<Share.IEntityHasTeacher> query;

    static Specification<Share.IEntityHasTeacher> specification;

    Establish context = () =>
    {
        Func<int, Mock<Share.IEntityHasTeacher>> entity = id =>
        {
            var mock = new Mock<Share.IEntityHasTeacher>();
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
}
