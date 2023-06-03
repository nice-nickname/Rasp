using Domain.Persistence.Specification;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.IEntityHasId), "Specification")]
class When_querying_entity_that_has_id
{
    static IQueryable<Share.IEntityHasId> query;

    Establish context = () =>
    {
        Func<int, Mock<Share.IEntityHasId>> entity = id =>
        {
            var mock = new Mock<Share.IEntityHasId>();
            mock.SetupProperty(s => s.Id, id);
            return mock;
        };

        query = new List<Share.IEntityHasId>
        {
                entity(1).Object,
                entity(2).Object,
                entity(3).Object,
                entity(1).Object
        }.AsQueryable();
    };

    class when_querying_by_id
    {
        Because of = () => query = query.Where(new Share.Where.ById<Share.IEntityHasId>(1)
                                                       .IsSatisfiedBy());

        It should_be_filtered = () => query.Count().ShouldEqual(2);
    }

    class when_querying_by_having_id
    {
        Because of = () => query = query.Where(new Share.Where.HasId<Share.IEntityHasId>(new[] { 1, 2 })
                                                       .IsSatisfiedBy());

        It should_be_filtered = () => query.Count().ShouldEqual(3);
    }
}
