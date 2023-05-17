using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;

namespace Domain.Api;

public class GetClassByWeekQuery : QueryBase<List<GetClassByWeekQuery.Response>>
{
    public int FacultyId { get; set; }

    public int Week { get; set; }

    public int?[] SelectedGroupIds { get; set; }

    public int?[] SelectedAuditoriumIds { get; set; }

    public int?[] SelectedTeacherIds { get; set; }

    protected override List<Response> ExecuteResult()
    {
        var res = new List<Response>();

        if (SelectedTeacherIds.Length <= 1 && SelectedAuditoriumIds.Length <= 1 && SelectedGroupIds.Length <= 1)
        {
            var scheduledAll = Repository.Query<Class>()
                                         .Where(r => r.Week == Week && r.ScheduleFormat.FacultyId == FacultyId);
            var disciplinePlansAll = Repository.Query<DisciplinePlan>();

            if (SelectedGroupIds.FirstOrDefault() != null)
            {
                scheduledAll = scheduledAll.Where(r => r.Plan.GroupId == SelectedGroupIds.FirstOrDefault());
                disciplinePlansAll = disciplinePlansAll.Where(r => r.GroupId == SelectedGroupIds.FirstOrDefault());
            }

            if (SelectedTeacherIds.FirstOrDefault() != null)
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
                                IsGroup = SelectedGroupIds.FirstOrDefault() != null,
                                IsAuditorium = SelectedAuditoriumIds.FirstOrDefault() != null,
                                IsTeacher = SelectedTeacherIds.FirstOrDefault() != null,
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
            typeOf? type = null;

            if (SelectedGroupIds.First() != null && SelectedGroupIds.Length > 1)
            {
                items = SelectedGroupIds.ToList();
                type = typeOf.Groups;
            }

            if (SelectedAuditoriumIds.First() != null && SelectedAuditoriumIds.Length > 1)
            {
                items = SelectedAuditoriumIds.ToList();
                type = typeOf.Auditoriums;
            }

            if (SelectedTeacherIds.First() != null && SelectedTeacherIds.Length > 1)
            {
                items = SelectedTeacherIds.ToList();
                type = typeOf.Teachers;
            }

            switch (type)
            {
                case typeOf.Groups:
                    scheduledAll = scheduledAll.Where(r => items.Contains(r.Plan.GroupId));
                    disciplinePlansAll = disciplinePlansAll.Where(r => items.Contains(r.GroupId));

                    break;

                case typeOf.Auditoriums:

                    break;

                case typeOf.Teachers:
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
                                IsTeacher = true,
                                AuditoriumId = SelectedAuditoriumIds.FirstOrDefault()
                        });

                        scheduledBySubGroupCount++;
                    }
                }
            }
        }

        return res;
    }

    private enum typeOf
    {
        Groups,

        Auditoriums,

        Teachers
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
