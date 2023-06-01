using Domain.Api;
using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.ViewModel;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;
using NHibernate.Hql.Ast.ANTLR.Tree;

namespace Domain.Test;

[Tags("UnitTest")]
[Subject(typeof(GetAuditoriumsScheduleForDDQuery), "Query")]
class When_executing_get_auditoriums_schedule_for_dd
{
    static Auditorium[] expected;
    
    static GetAuditoriumsScheduleForDDQuery query;
    
    static MockMessage<GetAuditoriumsScheduleForDDQuery, List<KeyValueVm>> mockQuery;
    
    Establish context = () =>
    {
        
    };

    Because of = () => mockQuery.Execute();

    //It should_return_list_of_departments_with_building = () => mockQuery.ShouldBeIsResult(list => list.ShouldEqualWeakEach(expected, (dsl, i) =>
    //{
        
    //}));
}
