using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;

namespace Domain.Api;

public class MarkTeacherPreferencesCommand : CommandBase
{
    public int TeacherId { get; set; }

    public DayOfWeek Day { get; set; }

    public int ScheduleFormatId { get; set; }

    public GetTeacherPreferencesQuery.PreferenceType Type { get; set; }

    protected override void Execute()
    {
        var preference = Repository.Query(new Share.Where.ByTeacher<TeacherPreferences>(TeacherId)
                                                  .And(new TeacherPreferences.Where.ByDay(Day, ScheduleFormatId)))
                                   .FirstOrDefault();

        if (preference != null && Type == GetTeacherPreferencesQuery.PreferenceType.NONE)
        {
            Repository.Delete<TeacherPreferences>(preference.Id);
            return;
        }

        preference ??= new TeacherPreferences { Day = Day, ScheduleFormatId = ScheduleFormatId, TeacherId = TeacherId };
        preference.Type = Type switch
        {
                GetTeacherPreferencesQuery.PreferenceType.IMPOSSIBLE => TeacherPreferences.PreferenceType.Impossible,
                GetTeacherPreferencesQuery.PreferenceType.UNWANTED => TeacherPreferences.PreferenceType.Unwanted,
                _ => throw new ArgumentOutOfRangeException()
        };
        Repository.SaveOrUpdate(preference);

        var classes = Repository.Query<Class>()
                                .Where(s => s.ScheduleFormatId == preference.ScheduleFormatId &&
                                            s.Plan.TeacherId == preference.TeacherId)
                                .ToList();

        foreach (var @class in classes)
        {
            @class.IsUnwanted = true;
        }
    }
}
