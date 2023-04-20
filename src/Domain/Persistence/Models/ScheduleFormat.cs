using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class ScheduleFormat : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual TimeSpan Start { get; set; }

    public virtual TimeSpan End { get; set; }

    public virtual int Order { get; set; }

    public virtual int FacultyId { get; set; }

    public virtual Faculty Faculty { get; set; }

    public class Mapping : ClassMap<ScheduleFormat>
    {
        public Mapping()
        {
            Table(nameof(ScheduleFormat));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Start).CustomType("TimeAsTimeSpan");
            Map(s => s.End).CustomType("TimeAsTimeSpan");
            Map(s => s.FacultyId);
            Map(s => s.Order);
            References(s => s.Faculty).Column(nameof(FacultyId))
                                      .ReadOnly()
                                      .LazyLoad();
        }
    }
}
