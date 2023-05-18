using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using static Domain.Api.GetScheduleByWeekQuery;

namespace Domain.Api;

public class GetClassByWeekQuery : QueryBase<List<GetClassByWeekQuery.Response>>
{
    public int FacultyId { get; set; }

    public int Week { get; set; }

    public int?[] SelectedGroupIds { get; set; }

    public int?[] SelectedAuditoriumIds { get; set; }

    public int?[] SelectedTeacherIds { get; set; }

    public ModeOf Mode { get; set; }

    protected override List<Response> ExecuteResult()
    {
        var res = new List<Response>();

        if (SelectedTeacherIds.Length <= 1 && SelectedAuditoriumIds.Length <= 1 && SelectedGroupIds.Length <= 1)
        {
            var scheduledAll = Repository.Query<Class>()
                                         .Where(r => r.Week == Week && r.ScheduleFormat.FacultyId == FacultyId);
            var disciplinePlansAll = Repository.Query<DisciplinePlan>();

            if (Mode is ModeOf.Groups)
            {
                scheduledAll = scheduledAll.Where(r => r.Plan.GroupId == SelectedGroupIds.FirstOrDefault());
                disciplinePlansAll = disciplinePlansAll.Where(r => r.GroupId == SelectedGroupIds.FirstOrDefault());
            }

            if (Mode is ModeOf.Teachers)
            {
                scheduledAll = scheduledAll.Where(r => r.Plan.TeacherId == SelectedTeacherIds.FirstOrDefault());
                disciplinePlansAll = disciplinePlansAll.Where(r => r.TeacherId == SelectedTeacherIds.FirstOrDefault());
            }

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
                var countToCreate = disciplinePlan.WeekAssignments.FirstOrDefault(s => s.Week == Week)!.AssignmentHours;
                var subGroupCount = disciplinePlan.SubGroupsCount;

                for (var i = 0; i < subGroupCount; i++)
                {
                    var subGroupNo = subGroupCount == 1 ? 0 : i + 1;
                    var scheduledBySubGroupCount = scheduled.Count(r => r.DisciplinePlanId == disciplinePlan.Id
                                                                     && r.SubGroupNo == subGroupNo
                                                                     && r.GroupId == disciplinePlan.GroupId);

                    for (var j = 0; j < countToCreate; j++)
                    {
                        if (scheduledBySubGroupCount == countToCreate)
                            break;

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
                                DisciplinePlanId = disciplinePlan.Id,
                                IsGroup = Mode is ModeOf.Groups,
                                IsAuditorium = Mode is ModeOf.Auditoriums,
                                IsTeacher = Mode is ModeOf.Teachers,
                                AuditoriumId = SelectedAuditoriumIds.FirstOrDefault()
                        });

                        scheduledBySubGroupCount++;
                    }
                }
            }
        }
        else
        {
            var scheduledAll = Repository.Query<Class>()
                                         .Where(r => r.Week == Week && r.ScheduleFormat.FacultyId == FacultyId);
            var disciplinePlansAll = Repository.Query<DisciplinePlan>();

            var items = new List<int?>();

            switch (Mode)
            {
                case ModeOf.Groups:
                    items = SelectedGroupIds.ToList();
                    scheduledAll = scheduledAll.Where(r => items.Contains(r.Plan.GroupId));
                    disciplinePlansAll = disciplinePlansAll.Where(r => items.Contains(r.GroupId));

                    break;

                case ModeOf.Auditoriums:
                    items = SelectedAuditoriumIds.ToList();

                    break;

                case ModeOf.Teachers:
                    items = SelectedTeacherIds.ToList();
                    scheduledAll = scheduledAll.Where(r => items.Contains(r.Plan.TeacherId));
                    disciplinePlansAll = disciplinePlansAll.Where(r => items.Contains(r.TeacherId));

                    break;
            }

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
                var countToCreate = disciplinePlan.WeekAssignments.FirstOrDefault(s => s.Week == Week)!.AssignmentHours;
                var subGroupCount = disciplinePlan.SubGroupsCount;

                for (var i = 0; i < subGroupCount; i++)
                {
                    var subGroupNo = subGroupCount == 1 ? 0 : i + 1;
                    var scheduledBySubGroupCount = scheduled.Count(r => r.DisciplinePlanId == disciplinePlan.Id
                                                                     && r.SubGroupNo == subGroupNo
                                                                     && r.GroupId == disciplinePlan.GroupId);

                    for (var j = 0; j < countToCreate; j++)
                    {
                        if (scheduledBySubGroupCount == countToCreate)
                            break;

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
                                DisciplinePlanId = disciplinePlan.Id,
                                IsGroup = true,
                                IsAuditorium = true,
                                IsTeacher = true
                        });

                        scheduledBySubGroupCount++;
                    }
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

        public int? AuditoriumId { get; set; }

        public bool HasSubGroups { get; set; }

        public bool IsGroup { get; set; }

        public bool IsAuditorium { get; set; }

        public bool IsTeacher { get; set; }
    }
}
