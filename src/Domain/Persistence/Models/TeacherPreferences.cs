using System.Linq.Expressions;
using Domain.Persistence.Mappers;
using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence;

public class TeacherPreferences : IncEntityBase, Share.IEntityHasId, Share.IEntityHasTeacher
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

    public virtual DayOfWeek Day { get; set; }

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
            Map(s => s.Day).CustomType<NHibernateDayOfWeekEnumMapper>();

            References(s => s.Teacher).Column(nameof(TeacherId))
                                      .ReadOnly()
                                      .LazyLoad();

            References(s => s.ScheduleFormat).Column(nameof(ScheduleFormatId))
                                             .ReadOnly()
                                             .LazyLoad();
        }
    }

    public class Where
    {
        public class ByDay : Specification<TeacherPreferences>
        {
            private readonly int _scheduleFormatId;

            private readonly DayOfWeek _day;

            public ByDay(DayOfWeek day, int scheduleFormatId)
            {
                this._day = day;
                this._scheduleFormatId = scheduleFormatId;
            }

            public override Expression<Func<TeacherPreferences, bool>> IsSatisfiedBy()
            {
                return s => s.Day == _day && s.ScheduleFormatId == _scheduleFormatId;
            }
        }
    }
}
