using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Newtonsoft.Json;
using NHibernate.Linq;
using System.Drawing;
using static Domain.Api.GetScheduleByWeekQuery;

namespace Domain.Api
{
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
            var scheduled = Repository.Query<Class>()
                .Where(r => r.Week == Week && r.ScheduleFormat.FacultyId == FacultyId);

            var disciplinePlans = Repository.Query<DisciplinePlan>();

            if (Mode is ModeOf.Groups)
            {
                scheduled = scheduled.Where(r => SelectedGroupIds.Contains(r.Plan.GroupId));
                disciplinePlans = disciplinePlans.Where(r => SelectedGroupIds.Contains(r.GroupId));
            }

            if (Mode is ModeOf.Teachers)
            {
                scheduled = scheduled.Where(r => SelectedTeacherIds.Contains(r.Plan.TeacherId));
                disciplinePlans = disciplinePlans.Where(r => SelectedTeacherIds.Contains(r.TeacherId));
            }

            disciplinePlans.FetchMany(s => s.WeekAssignments);

            var res = new List<Response>();

            foreach (var disciplinePlan in disciplinePlans)
            {
                var countToCreate = disciplinePlan.WeekAssignments.FirstOrDefault(s => s.Week == Week)?.AssignmentHours ?? 0;
                var subGroupCount = disciplinePlan.SubGroupsCount;

                for (var i = 0; i < subGroupCount; i++)
                {
                    var subGroupNo = subGroupCount == 1 ? 0 : i + 1;
                    var scheduledBySubGroupCount = scheduled.Count(r => r.DisciplinePlanId == disciplinePlan.Id &&
                                                                        r.SubGroupNo == subGroupNo &&
                                                                        r.Plan.GroupId == disciplinePlan.GroupId);

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
                            Color = ColorTranslator.ToHtml(disciplinePlan.SubDiscipline.Kind.Color),
                            SubDisciplineCode = disciplinePlan.SubDiscipline.Kind.Code,
                            HasSubGroups = subGroupCount != 1,
                            DisciplinePlanId = disciplinePlan.Id,
                            IsGroup = Mode is ModeOf.Groups,
                            IsAuditorium = Mode is ModeOf.Auditoriums,
                            IsTeacher = Mode is ModeOf.Teachers,
                            AuditoriumId = SelectedAuditoriumIds?.FirstOrDefault()
                        });

                        scheduledBySubGroupCount++;
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
            
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
            public int SubGroupNo { get; set; }
            
            public int DisciplinePlanId { get; set; }
            
            public int? AuditoriumId { get; set; }
            
            public bool HasSubGroups { get; set; }
            
            public bool IsGroup { get; set; }
            
            public bool IsAuditorium { get; set; }
            
            public bool IsTeacher { get; set; }
        }
    }
}
