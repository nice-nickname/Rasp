using Domain.Persistence;
using Domain.Persistence.Specification;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.IEntityHasDepartment), "Specification")]
class When_querying_entity_that_has_department
{
    static IQueryable<Share.IEntityHasDepartment> query;

    Establish context = () =>
    {
        Func<int, Mock<Share.IEntityHasDepartment>> entity = id =>
        {
            var mock = new Mock<Share.IEntityHasDepartment>();
            mock.SetupProperty(s => s.DepartmentId, id);
            mock.SetupProperty(s => s.Department, new Department { Id = id, FacultyId = id});
            return mock;
        };

        query = new List<Share.IEntityHasDepartment>
        {
                entity(1).Object,
                entity(2).Object,
                entity(3).Object,
                entity(1).Object
        }.AsQueryable();
    };

    class when_querying_by_group
    {
        Because of = () => query = query.Where(new Share.Where.ByDepartment<Share.IEntityHasDepartment>(1)
                                                       .IsSatisfiedBy());

        It should_be_filtered = () => query.Count().ShouldEqual(2);
    }

    class when_querying_by_faculty_through_department
    {
        Because of = () => query = query.Where(new Share.Where.ByFacultyThoughDepartment<Share.IEntityHasDepartment>(1)
                                                       .IsSatisfiedBy());

        It should_be_filtered = () => query.Count().ShouldEqual(2);
    }
}
