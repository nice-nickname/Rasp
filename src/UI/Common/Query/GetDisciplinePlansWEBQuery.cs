using Domain.Api;
using Incoding.Core.CQRS.Core;
using UI.Common.Models;

namespace UI.Query;

public class GetDisciplinePlansWEBQuery : QueryBase<LastIndexModel<GetDisciplinePlanQuery.Response>>
{
    public int FacultyId { get; set; }

    public int? SubDisciplineId { get; set; }

    public int[]? GroupIds { get; set; }

    public int[]? TeacherIds { get; set; }

    public int LastIndex { get; set; }

    public int Hours { get; set; }

    protected override LastIndexModel<GetDisciplinePlanQuery.Response> ExecuteResult()
    {
        return new LastIndexModel<GetDisciplinePlanQuery.Response>
        {
                Items = Dispatcher.Query(new GetDisciplinePlanQuery
                {
                        FacultyId = FacultyId,
                        SubDisciplineId = SubDisciplineId,
                        GroupIds = GroupIds,
                        TeacherIds = TeacherIds,
                        TotalHours = Hours
                }),
                LastIndex = LastIndex
        };
    }
}
