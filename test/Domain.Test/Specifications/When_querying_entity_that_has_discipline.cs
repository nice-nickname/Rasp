using Domain.Persistence.Specification;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.IEntityHasDiscipline), "Specification")]
class When_querying_entity_that_has_discipline
{
    static IQueryable<Share.IEntityHasDiscipline> query;

    Establish context = () =>
    {
        Func<int, Mock<Share.IEntityHasDiscipline>> entity = id =>
        {
            var mock = new Mock<Share.IEntityHasDiscipline>();
            mock.SetupProperty(s => s.DisciplineId, id);
            return mock;
        };

        query = new List<Share.IEntityHasDiscipline>
        {
                entity(1).Object,
                entity(2).Object,
                entity(3).Object,
                entity(1).Object
        }.AsQueryable();
    };

    Because of = () => query = query.Where(new Share.Where.ByDiscipline<Share.IEntityHasDiscipline>(1)
                                                   .IsSatisfiedBy());

    It should_be_filtered = () => query.Count().ShouldEqual(2);
}
