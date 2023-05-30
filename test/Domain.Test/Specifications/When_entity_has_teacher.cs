using Domain.Persistence.Specification;
using Machine.Specifications;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(Share.Where.ByTeacher<>), "Specification")]
class When_entity_has_teacher
{
    Establish context = () =>
    {
    };

    Because of = () => {};

    It should_be_filtered = () => {};
}

