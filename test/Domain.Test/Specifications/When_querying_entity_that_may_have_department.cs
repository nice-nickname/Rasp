using Domain.Persistence.Specification;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.IEntityMayHaveDepartment), "Specification")]
class When_querying_entity_that_may_have_department
{
    static IQueryable<Share.IEntityMayHaveDepartment> query;

    Establish context = () =>
    {
        Func<int, Mock<Share.IEntityMayHaveDepartment>> entity = id =>
        {
            var mock = new Mock<Share.IEntityMayHaveDepartment>();
            mock.SetupProperty(s => s.DepartmentId, id);
            return mock;
        };

        query = new List<Share.IEntityMayHaveDepartment>
        {
                entity(1).Object,
                entity(2).Object,
                entity(3).Object,
                entity(1).Object
        }.AsQueryable();
    };

    Because of = () => query = query.Where(new Share.Where.ByNullableDepartment<Share.IEntityMayHaveDepartment>(1)
                                                   .IsSatisfiedBy());

    It should_be_filtered = () => query.Count().ShouldEqual(2);
}
