using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;

namespace Domain.Api;

public class GetDisciplinePlanQuery : QueryBase<List<GetDisciplinePlanQuery.Response>>
{
    public int FacultyId { get; set; }

    public int? SubDisciplineId { get; set; }

    public int[]? GroupIds { get; set; }

    public int[]? TeacherIds { get; set; }

    public int TotalHours { get; set; }

    protected override List<Response> ExecuteResult()
    {
        if (TeacherIds == null || GroupIds == null || !GroupIds.Any() || !TeacherIds.Any())
        {
            return new List<Response>();
        }

        var weeksCount = Dispatcher.Query(new GetFacultySettingCommand<int>
        {
                FacultyId = FacultyId,
                Type = FacultySettings.OfType.CountOfWeeks
        });

        var teachers = Repository.Query<Teacher>()
                                 .Where(s => TeacherIds.Contains(s.Id))
                                 .ToDictionary(k => k.Id);

        var groups = Repository.Query<Group>()
                               .Where(s => GroupIds.Contains(s.Id))
                               .ToDictionary(k => k.Id);

        var defaultWeek = Enumerable.Range(1, weeksCount)
                                    .Select(s => new WeekItem { Week = s, Hours = 0 })
                                    .ToList();

        var defaultTeachers = TeacherIds.Select(q => new Item
        { 
                TeacherId = q,
                Teacher = teachers[q].Name,
                WeekItems = defaultWeek,
                HoursAssigned = 0,
        }).ToList();

        var header = Enumerable.Range(1, weeksCount)
                               .Select(s => new HeaderWeek
                               {
                                       Week = s
                               })
                               .ToList();

        if (!SubDisciplineId.HasValue)
        {
            return GroupIds.Select(s => new Response
            {
                    GroupId = s,
                    Group = groups[s].Code,
                    SubGroupCount = 1,
                    TeacherHoursByWeeks = defaultTeachers,
                    Header = header,
                    TotalHours = TotalHours,
                    TotalAssignedHours = 0
            }).ToList();
        }

        var result = new List<Response>();

        var plans = Repository.Query(new Share.Where.BySubDiscipline<DisciplinePlan>(SubDisciplineId.Value)
                                             .And(new Share.Where.HasGroup<DisciplinePlan>(GroupIds)))
                              .GroupBy(s => new
                              {
                                      GroupId = s.GroupId,
                                      SubGroupCount = s.SubGroupsCount
                              })
                              .ToList();
        if (plans.Any())
        {
            result.AddRange(plans.Select(s =>
            {
                var teacherItems = s.Select(c => new Item
                {
                        Teacher = c.Teacher.Name,
                        TeacherId = c.TeacherId,
                        Id = c.Id,
                        WeekItems = c.WeekAssignments.Select(w => new WeekItem
                                     {
                                             Hours = w.AssignmentHours, Week = w.Week
                                     })
                                     .ToList(),
                        HoursAssigned = c.WeekAssignments.Sum(s => s.AssignmentHours),
                }).ToList();

                teacherItems.AddRange(TeacherIds.Except(teacherItems.Select(с => с.TeacherId))
                                                .Select(c => new Item
                                                {
                                                        Teacher = teachers[c].Name,
                                                        TeacherId = c,
                                                        WeekItems = defaultWeek
                                                }));

                return new Response
                {
                        Group = groups[s.Key.GroupId].Code,
                        GroupId = s.Key.GroupId,
                        Header = header,
                        SubGroupCount = s.Key.SubGroupCount,
                        TeacherHoursByWeeks = teacherItems,
                        TotalHours = TotalHours,
                        TotalAssignedHours = teacherItems.Sum(s => s.HoursAssigned)
                };
            }).ToList());
        }

        result = result.FindAll(s => GroupIds.Contains(s.GroupId));

        var missedGroups = GroupIds.Except(plans.Select(s => s.Key.GroupId));

        result.AddRange(missedGroups.Select(group => new Response
        {
                GroupId = group,
                Group = groups[group].Code,
                Header = header,
                SubGroupCount = 0,
                TeacherHoursByWeeks = defaultTeachers
        }));

        return result;
    }

    public record Response
    {
        public int GroupId { get; set; }

        public string Group { get; set; }

        public int SubGroupCount { get; set; }

        public int TotalAssignedHours { get; set; }

        public int TotalHours { get; set; }

        public List<Item> TeacherHoursByWeeks { get; set; }

        public List<HeaderWeek> Header { get; set; }
    }

    public record Item
    {
        public int? Id { get; set; }

        public int TeacherId { get; set; }

        public string Teacher { get; set; }

        public int HoursAssigned { get; set; }

        public List<WeekItem> WeekItems { get; set; }
    }

    public record WeekItem
    {
        public int Week { get; set; }

        public int Hours { get; set; }
    }

    public record HeaderWeek
    {
        public int Week { get; set; }
    }
}
