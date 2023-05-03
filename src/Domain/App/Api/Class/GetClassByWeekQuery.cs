using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetClassByWeekQuery : QueryBase<List<GetClassByWeekQuery.Response>>
{
    public int FacultyId { get; set; }

    public int Week { get; set; }

    protected override List<Response> ExecuteResult()
    {
        var res = new List<Response>();

        var disciplines = Repository.Query<DisciplinePlan>().ToList();

        foreach (var disciplinePlan in disciplines)
        {
            var countToCreate = disciplinePlan.WeekAssignments.FirstOrDefault(s => s.Week == Week)!.AssignmentHours;
            var subGroupCount = disciplinePlan.SubGroupsCount;

            for (var i = 0; i < subGroupCount; i++)
            {
                for (var j = 0; j < countToCreate; j++)
                {
                    res.Add(new Response
                    {
                            SubDisciplineId = disciplinePlan.SubDiscipline.Id,
                            DisciplineId = disciplinePlan.SubDiscipline.DisciplineId,
                            GroupId = disciplinePlan.GroupId,
                            TeacherId = disciplinePlan.TeacherId,
                            SubGroupNo = subGroupCount == 1 ? 0 : i + 1,
                            Discipline = disciplinePlan.SubDiscipline.Discipline.Name,
                            DisciplineCode = disciplinePlan.SubDiscipline.Discipline.Code,
                            SubDiscipline = disciplinePlan.SubDiscipline.Kind.Name,
                            Group = disciplinePlan.Group.Code,
                            Teacher = disciplinePlan.Teacher.ShortName,
                            Department = disciplinePlan.Teacher.Department.Name,
                            DepartmentCode = disciplinePlan.Teacher.Department.Code
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

        public int GroupId { get; set; }

        public int DisciplineId { get; set; }

        public int SubDisciplineId { get; set; }

        public int TeacherId { get; set; }

        public int SubGroupNo { get; set; }
    }
}
