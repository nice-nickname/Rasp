using Domain.Persistence.Specification;
using Machine.Specifications;
using Moq;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.Where.ByTeacher<>), "Specification")]
class When_entity_has_teacher
{
    // Establish context = () =>
    // {
    //     entities = Mocks.Of<Share.IEntityHasTeacher>();
    //     teacherId = entities.First().TeacherId;

    //     mock = Mock.Get(entities);
    // };

    // Because of = () => entities.Where(new Share.Where.ByTeacher<Share.IEntityHasTeacher>(teacherId).IsSatisfiedBy());

    // It should_be_filtered = () => mock.Verify();

    // static IQueryable<Share.IEntityHasTeacher> entities;

    // static Mock<IQueryable<Share.IEntityHasTeacher>> mock;

    // static int teacherId;
}

