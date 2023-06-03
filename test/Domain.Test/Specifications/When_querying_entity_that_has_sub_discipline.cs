using Domain.Persistence.Specification;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.IEntityHasSubDiscipline), "Specification")]
class When_querying_entity_that_has_sub_discipline
{
    static IQueryable<Share.IEntityHasSubDiscipline> query;

    Establish context = () =>
    {
        Func<int, Mock<Share.IEntityHasSubDiscipline>> entity = id =>
        {
            var mock = new Mock<Share.IEntityHasSubDiscipline>();
            mock.SetupProperty(s => s.SubDisciplineId, id);
            return mock;
        };

        query = new List<Share.IEntityHasSubDiscipline>
        {
                entity(1).Object,
                entity(2).Object,
                entity(3).Object,
                entity(1).Object
        }.AsQueryable();
    };

    Because of = () => query = query.Where(new Share.Where.BySubDiscipline<Share.IEntityHasSubDiscipline>(1)
                                                   .IsSatisfiedBy());

    It should_be_filtered = () => query.Count().ShouldEqual(2);
}
