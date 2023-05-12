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

        var response = new List<Response>();
        response.AddRange(new[]
        {
                new Response
                {
                        Day = DayOfWeek.Monday,
                        DayName = DataResources.Monday,
                        Days = scheduleFormat.Items
                                             .Select(s => new DayBusyness
                                             {
                                                     ScheduleItemId = s.Id.GetValueOrDefault(),
                                                     Type = PreferenceType.NONE
                                             })
                                             .ToArray()
                },
                new Response
                {
                        Day = DayOfWeek.Tuesday,
                        DayName = DataResources.Tuesday,
                        Days = scheduleFormat.Items
                                             .Select(s => new DayBusyness
                                             {
                                                     ScheduleItemId = s.Id.GetValueOrDefault(),
                                                     Type = PreferenceType.NONE
                                             })
                                             .ToArray()
                },
                new Response
                {
                        Day = DayOfWeek.Wednesday,
                        DayName = DataResources.Wednesday,
                        Days = scheduleFormat.Items
                                             .Select(s => new DayBusyness
                                             {
                                                     ScheduleItemId = s.Id.GetValueOrDefault(),
                                                     Type = PreferenceType.NONE
                                             })
                                             .ToArray()
                },
                new Response
                {
                        Day = DayOfWeek.Thursday,
                        DayName = DataResources.Thursday,
                        Days = scheduleFormat.Items
                                             .Select(s => new DayBusyness
                                             {
                                                     ScheduleItemId = s.Id.GetValueOrDefault(),
                                                     Type = PreferenceType.NONE
                                             })
                                             .ToArray()
                },
                new Response
                {
                        Day = DayOfWeek.Friday,
                        DayName = DataResources.Friday,
                        Days = scheduleFormat.Items
                                             .Select(s => new DayBusyness
                                             {
                                                     ScheduleItemId = s.Id.GetValueOrDefault(),
                                                     Type = PreferenceType.NONE
                                             })
                                             .ToArray()
                },
                new Response
                {
                        Day = DayOfWeek.Saturday,
                        DayName = DataResources.Saturday,
                        Days = scheduleFormat.Items
                                             .Select(s => new DayBusyness
                                             {
                                                     ScheduleItemId = s.Id.GetValueOrDefault(),
                                                     Type = PreferenceType.NONE
                                             })
                                             .ToArray()
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
                var preferenceIndex = Array.FindIndex(item.Days, 0, item.Days.Length, s => s.ScheduleItemId == preferenceItem.ScheduleFormatId);
                if (preferenceIndex != -1)
                {
                    item.Days[preferenceIndex].Type = preferenceItem.Type switch
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

        public DayBusyness[] Days { get; set; }
    }

    public struct DayBusyness
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
