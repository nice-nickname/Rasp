using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Class : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int GroupId { get; set; }

    public virtual int AuditoriumId { get; set; }

    public virtual int SubDisciplineId { get; set; }

    public virtual int ScheduleFormatId { get; set; }

    public virtual DateTime Day { get; set; }

    public virtual Auditorium Auditorium { get; set; }

    public virtual Group Group { get; set; }

    public virtual SubDiscipline SubDiscipline { get; set; }

    public virtual ScheduleFormat ScheduleFormat { get; set; }

    public class Mapping : ClassMap<Class>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.GroupId);
            Map(s => s.AuditoriumId);
            Map(s => s.SubDisciplineId);
            Map(s => s.ScheduleFormatId);
            Map(s => s.Day);

            References(s => s.Auditorium).Column(nameof(AuditoriumId))
                                         .ReadOnly()
                                         .LazyLoad();

            References(s => s.Group).Column(nameof(GroupId))
                                    .ReadOnly()
                                    .LazyLoad();

            References(s => s.SubDiscipline).Column(nameof(SubDisciplineId))
                                            .ReadOnly()
                                            .LazyLoad();

            References(s => s.ScheduleFormat).Column(nameof(ScheduleFormatId))
                                             .ReadOnly()
                                             .LazyLoad();
        }
    }
}