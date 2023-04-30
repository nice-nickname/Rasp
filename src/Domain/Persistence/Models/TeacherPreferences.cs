using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class TeacherPreferences : IncEntityBase
{
    public enum PreferenceType
    {
        Impossible,

        Unwanted
    }

    public new virtual int Id { get; set; }

    public virtual int TeacherId { get; set; }

    public virtual int ScheduleFormatId { get; set; }

    public virtual PreferenceType Type { get; set; }

    public virtual Teacher Teacher { get; set; }

    public virtual ScheduleFormat ScheduleFormat { get; set; }

    public class Mapping : ClassMap<TeacherPreferences>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.TeacherId);
            Map(s => s.ScheduleFormatId);
            Map(s => s.Type).CustomType<PreferenceType>();

            References(s => s.Teacher).Column(nameof(TeacherId))
                                      .ReadOnly()
                                      .LazyLoad();

            References(s => s.ScheduleFormat).Column(nameof(ScheduleFormatId))
                                             .ReadOnly()
                                             .LazyLoad();
        }
    }
}
