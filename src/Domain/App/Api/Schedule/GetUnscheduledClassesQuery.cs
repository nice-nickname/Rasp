using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;

namespace Domain.Api;

public class GetUnscheduledClassesQuery : QueryBase<List<GetUnscheduledClassesQuery.Response>>
{
    public int FacultyId { get; set; }

    protected override List<Response> ExecuteResult()
    {
        var res = new List<Response>();
        var scheduledAll = Repository.Query<Class>()
                                     .Where(r => r.ScheduleFormat.FacultyId == FacultyId);
        var disciplinePlansAll = Repository.Query<DisciplinePlan>();

        var scheduled = scheduledAll.Select(r => new
                                    {
                                            r.DisciplinePlanId,
                                            r.SubGroupNo,
                                            r.Plan.GroupId
                                    })
                                    .ToList();

        var disciplinePlans = disciplinePlansAll.ToList();

        foreach (var disciplinePlan in disciplinePlans)
        {
            var week = 1;

            foreach (var weekAssignment in disciplinePlan.WeekAssignments)
            {
                var countToCreate = weekAssignment.AssignmentHours;
                var subGroupCount = disciplinePlan.SubGroupsCount;

                for (var i = 0; i < subGroupCount; i++)
                {
                    var subGroupNo = subGroupCount == 1 ? 0 : i + 1;
                    var scheduledBySubGroupCount = scheduled.Count(r => r.DisciplinePlanId == disciplinePlan.Id
                                                                     && r.SubGroupNo == subGroupNo
                                                                     && r.GroupId == disciplinePlan.GroupId);

                    for (var j = 0; j < countToCreate; j++)
                    {
                        if (scheduledBySubGroupCount == countToCreate || res.Count >= 7)
                            break;

                        res.Add(new Response
                        {
                                GroupId = disciplinePlan.GroupId,
                                Week = week,
                                Group = disciplinePlan.Group.Code,
                                SubGroupNo = subGroupNo,
                                Teacher = disciplinePlan.Teacher.Name,
                                TeacherShort = disciplinePlan.Teacher.ShortName,
                                Discipline = disciplinePlan.SubDiscipline.Discipline.Name,
                                DisciplineShort = disciplinePlan.SubDiscipline.Discipline.Code,
                                SubDisciplineKind = disciplinePlan.SubDiscipline.Kind.Name,
                                SubDisciplineKindShort = disciplinePlan.SubDiscipline.Kind.Code,
                                Color = disciplinePlan.SubDiscipline.Kind.Color.ToHex(),
                                Department = disciplinePlan.SubDiscipline.Discipline.Department?.Name,
                                DepartmentShort = disciplinePlan.SubDiscipline.Discipline.Department.Code,
                                HasSubGroups = subGroupCount > 1
                        });

                        scheduledBySubGroupCount++;
                    }
                }

                week++;
            }
        }

        return res;
    }

    public class Response
    {
        public int GroupId { get; set; }

        public int TeacherId { get; set; }

        public int Week { get; set; }

        public int SubGroupNo { get; set; }

        public string Group { get; set; }

        public string Teacher { get; set; }

        public string TeacherShort { get; set; }

        public string Discipline { get; set; }

        public string DisciplineShort { get; set; }

        public string SubDisciplineKind { get; set; }

        public string SubDisciplineKindShort { get; set; }

        public string Color { get; set; }

        public string Department { get; set; }

        public string DepartmentShort { get; set; }

        public bool HasSubGroups { get; set; }
    }
}
