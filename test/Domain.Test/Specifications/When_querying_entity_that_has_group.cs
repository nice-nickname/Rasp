using Domain.Persistence.Specification;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.IEntityHasGroup), "Specification")]
class When_querying_entity_that_has_group
{
    static IQueryable<Share.IEntityHasGroup> query;

    Establish context = () =>
    {
        Func<int, Mock<Share.IEntityHasGroup>> entity = id =>
        {
            var mock = new Mock<Share.IEntityHasGroup>();
            mock.SetupProperty(s => s.GroupId, id);
            return mock;
        };

        query = new List<Share.IEntityHasGroup>
        {
                entity(1).Object,
                entity(2).Object,
                entity(3).Object,
                entity(1).Object
        }.AsQueryable();
    };

    class when_querying_by_group
    {
        Because of = () => query = query.Where(new Share.Where.ByGroup<Share.IEntityHasGroup>(1)
                                                       .IsSatisfiedBy());

        It should_be_filtered = () => query.Count().ShouldEqual(2);
    }

    class when_querying_by_having_group
    {
        Because of = () => query = query.Where(new Share.Where.HasGroup<Share.IEntityHasGroup>(new[] { 1, 2 })
                                                       .IsSatisfiedBy());

        It should_be_filtered = () => query.Count().ShouldEqual(3);
    }
}
