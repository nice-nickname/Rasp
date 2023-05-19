using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;

namespace Domain.Api;

public class GetUnwantedClassesQuery : QueryBase<List<GetUnscheduledClassesQuery.Response>>
{
    public int FacultyId { get; set; }

    protected override List<GetUnscheduledClassesQuery.Response> ExecuteResult()
    {
        return Repository.Query<Class>()
                         .Where(r => r.IsUnwanted == true)
                         .ToList()
                         .Where(r => r.ScheduleFormat.FacultyId == FacultyId)
                         .Take(7)
                         .Select(r => new GetUnscheduledClassesQuery.Response
                         {
                                 TeacherId = r.Plan.TeacherId,
                                 Week = r.Week,
                                 Group = r.Plan.Group.Code,
                                 SubGroupNo = r.SubGroupNo,
                                 Teacher = r.Plan.Teacher.Name,
                                 TeacherShort = r.Plan.Teacher.ShortName,
                                 Discipline = r.Plan.SubDiscipline.Discipline.Name,
                                 DisciplineShort = r.Plan.SubDiscipline.Discipline.Code,
                                 SubDisciplineKind = r.Plan.SubDiscipline.Kind.Name,
                                 SubDisciplineKindShort = r.Plan.SubDiscipline.Kind.Code,
                                 Color = r.Plan.SubDiscipline.Kind.Color.ToHex(),
                                 Department = r.Plan.SubDiscipline.Discipline.Department?.Name,
                                 DepartmentShort = r.Plan.SubDiscipline.Discipline.Department.Code,
                                 HasSubGroups = r.SubGroupNo > 1
                         })
                         .ToList();
    }
}
