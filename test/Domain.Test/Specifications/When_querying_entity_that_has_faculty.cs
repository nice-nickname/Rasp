using Domain.Persistence.Specification;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.IEntityHasFaculty), "Specification")]
class When_querying_entity_that_has_faculty
{
    static IQueryable<Share.IEntityHasFaculty> query;

    Establish context = () =>
    {
        Func<int, Mock<Share.IEntityHasFaculty>> entity = id =>
        {
            var mock = new Mock<Share.IEntityHasFaculty>();
            mock.SetupProperty(s => s.FacultyId, id);
            return mock;
        };

        query = new List<Share.IEntityHasFaculty>
        {
                entity(1).Object,
                entity(2).Object,
                entity(3).Object,
                entity(1).Object
        }.AsQueryable();
    };

    Because of = () => query = query.Where(new Share.Where.ByFaculty<Share.IEntityHasFaculty>(1)
                                                   .IsSatisfiedBy());

    It should_be_filtered = () => query.Count().ShouldEqual(2);
}
