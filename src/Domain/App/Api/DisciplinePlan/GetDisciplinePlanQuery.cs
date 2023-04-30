using System.Net.Http.Headers;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetDisciplinePlanQuery : QueryBase<List<GetDisciplinePlanQuery.Response>>
{
    public int FacultyId { get; set; }

    public int? SubDisciplineId { get; set; }

    public int[]? GroupIds { get; set; }

    public int[]? TeacherIds { get; set; }

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
                    TeacherHoursByWeeks = TeacherIds.Select(q => new Item
                    {
                            TeacherId = q,
                            Teacher = teachers[q].Name,
                            WeekItems = defaultWeek
                    }).ToList(),
                    Header = header
            }).ToList();
        }

        throw new NotImplementedException();
    }

    public record Response
    {
        public int GroupId { get; set; }

        public string Group { get; set; }

        public int SubGroupCount { get; set; }

        public List<Item> TeacherHoursByWeeks { get; set; }

        public List<HeaderWeek> Header { get; set; }
    }

    public record Item
    {
        public int TeacherId { get; set; }

        public string Teacher { get; set; }

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
