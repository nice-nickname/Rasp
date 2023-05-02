using Domain.Persistence.Mappers;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Class : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int Week { get; set; }

    public virtual DayOfWeek Day { get; set; }

    public virtual int SubGroupNo { get; set; }

    public virtual int AuditoriumId { get; set; }

    public virtual int ScheduleFormatId { get; set; }

    public virtual int DisciplinePlanId { get; set; }

    public virtual Auditorium Auditorium { get; set; }

    public virtual ScheduleFormat ScheduleFormat { get; set; }

    public virtual DisciplinePlan Plan { get; set; }

    public class Mapping : ClassMap<Class>
    {
        public Mapping()
        {
            Table(nameof(Class));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Week);
            Map(s => s.SubGroupNo);
            Map(s => s.Day).CustomType<NHibernateDayOfWeekEnumMapper>().Not.Nullable();
            Map(s => s.AuditoriumId);
            Map(s => s.ScheduleFormatId);
            Map(s => s.DisciplinePlanId);

            References(s => s.Auditorium).Column(nameof(AuditoriumId))
                                         .ReadOnly()
                                         .LazyLoad();

            References(s => s.ScheduleFormat).Column(nameof(ScheduleFormatId))
                                             .ReadOnly()
                                             .LazyLoad();

            References(s => s.Plan).Column(nameof(DisciplinePlanId))
                                   .ReadOnly()
                                   .LazyLoad();
        }
    }
}
