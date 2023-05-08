using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;

namespace Domain.Api;

public class GetClassByWeekQuery : QueryBase<List<GetClassByWeekQuery.Response>>
{
    public int FacultyId { get; set; }

    public int Week { get; set; }

    public int GroupId { get; set; }

    protected override List<Response> ExecuteResult()
    {
        var res = new List<Response>();

        var scheduled = Repository.Query<Class>()
                                  .Where(r => r.ScheduleFormat.FacultyId == FacultyId
                                           && r.Plan.GroupId == GroupId)
                                  .Select(r => new
                                  {
                                          r.DisciplinePlanId,
                                          r.SubGroupNo
                                  })
                                  .ToList();

        var disciplinePlans = Repository.Query<DisciplinePlan>()
                                        .Where(r => r.GroupId == GroupId)
                                        .ToList();

        foreach (var disciplinePlan in disciplinePlans)
        {
            var countToCreate = disciplinePlan.WeekAssignments.FirstOrDefault(s => s.Week == Week)!.AssignmentHours;
            var subGroupCount = disciplinePlan.SubGroupsCount;

            for (var i = 0; i < subGroupCount; i++)
            {
                for (var j = 0; j < countToCreate; j++)
                {
                    var subGroupNo = subGroupCount == 1 ? 0 : i + 1;

                    if (scheduled.Exists(r => r.DisciplinePlanId == disciplinePlan.Id
                                           && r.SubGroupNo == subGroupNo))
                        continue;

                    res.Add(new Response
                    {
                            SubDisciplineId = disciplinePlan.SubDiscipline.Id,
                            DisciplineId = disciplinePlan.SubDiscipline.DisciplineId,
                            GroupId = disciplinePlan.GroupId,
                            TeacherId = disciplinePlan.TeacherId,
                            SubGroupNo = subGroupNo,
                            Discipline = disciplinePlan.SubDiscipline.Discipline.Name,
                            DisciplineCode = disciplinePlan.SubDiscipline.Discipline.Code,
                            SubDiscipline = disciplinePlan.SubDiscipline.Kind.Name,
                            Group = disciplinePlan.Group.Code,
                            Teacher = disciplinePlan.Teacher.ShortName,
                            Department = disciplinePlan.Teacher.Department.Name,
                            DepartmentCode = disciplinePlan.Teacher.Department.Code,
                            Color = disciplinePlan.SubDiscipline.Kind.Color.ToHex(),
                            SubDisciplineCode = disciplinePlan.SubDiscipline.Kind.Code,
                            HasSubGroups = subGroupCount != 1,
                            DisciplinePlanId = disciplinePlan.Id
                    });
                }
            }
        }

        return res;
    }

    public record Response
    {
        public string Group { get; set; }

        public string Discipline { get; set; }

        public string DisciplineCode { get; set; }

        public string SubDiscipline { get; set; }

        public string Teacher { get; set; }

        public string Department { get; set; }

        public string DepartmentCode { get; set; }

        public string Color { get; set; }

        public string SubDisciplineCode { get; set; }

        public int GroupId { get; set; }

        public int DisciplineId { get; set; }

        public int SubDisciplineId { get; set; }

        public int TeacherId { get; set; }

        public int SubGroupNo { get; set; }

        public int DisciplinePlanId { get; set; }

        public bool HasSubGroups { get; set; }
    }
}
