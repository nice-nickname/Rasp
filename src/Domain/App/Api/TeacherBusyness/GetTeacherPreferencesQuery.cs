using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.Api;

public class GetTeacherPreferencesQuery : QueryBase<List<GetTeacherPreferencesQuery.Response>>
{
    public int FacultyId { get; set; }

    public int? TeacherId { get; set; }

    protected override List<Response> ExecuteResult()
    {
        var scheduleFormat = Dispatcher.Query(new GetScheduleFormatQuery
        {
                FacultyId = FacultyId
        });

        var defaultDays = scheduleFormat.Items
                                        .Select(s => new DayBusyness
                                        {
                                                ScheduleItemId = s.Id,
                                                Type = PreferenceType.NONE
                                        })
                                        .ToList();

        var response = new List<Response>();
        response.AddRange(new[]
        {
                new Response
                {
                        Day = DayOfWeek.Monday,
                        DayName = DataResources.Monday,
                        Days = defaultDays
                },
                new Response
                {
                        Day = DayOfWeek.Tuesday,
                        DayName = DataResources.Tuesday,
                        Days = defaultDays
                },
                new Response
                {
                        Day = DayOfWeek.Wednesday,
                        DayName = DataResources.Wednesday,
                        Days = defaultDays
                },
                new Response
                {
                        Day = DayOfWeek.Thursday,
                        DayName = DataResources.Thursday,
                        Days = defaultDays
                },
                new Response
                {
                        Day = DayOfWeek.Friday,
                        DayName = DataResources.Friday,
                        Days = defaultDays
                },
                new Response
                {
                        Day = DayOfWeek.Saturday,
                        DayName = DataResources.Saturday,
                        Days = defaultDays
                },
                new Response
                {
                        Day = DayOfWeek.Sunday,
                        DayName = DataResources.Sunday,
                        Days = defaultDays
                }
        });

        if (!TeacherId.HasValue)
        {
            return response;
        }

        foreach (var preference in Repository.Query(new Share.Where.ByTeacher<TeacherPreferences>(TeacherId.Value))
                                             .GroupBy(s => s.Day))
        {
            var item = response.First(s => s.Day == preference.Key);
            foreach (var preferenceItem in preference)
            {
                var pref = item.Days.FirstOrDefault(s => s.ScheduleItemId == preferenceItem.ScheduleFormatId);
                if (pref != null)
                {
                    pref.Type = preferenceItem.Type switch
                    {
                            TeacherPreferences.PreferenceType.Impossible => PreferenceType.IMPOSSIBLE,
                            TeacherPreferences.PreferenceType.Unwanted => PreferenceType.UNWANTED,
                            _ => PreferenceType.NONE
                    };
                }
            }
        }

        return response;
    }

    public record Response
    {
        public DayOfWeek Day { get; set; }

        public string DayName { get; set; }

        public List<DayBusyness> Days { get; set; }
    }

    public record DayBusyness
    {
        public int ScheduleItemId { get; set; }

        public PreferenceType Type { get; set; }
    }

    public enum PreferenceType
    {
        NONE,

        UNWANTED,

        IMPOSSIBLE
    }
}
