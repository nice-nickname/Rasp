using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class ScheduleFormat : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual DateTime Start { get; set; }

    public virtual DateTime End { get; set; }

    public virtual int Order { get; set; }

    public virtual int FacultyId { get; set; }

    public virtual Faculty Faculty { get; set; }

    public class Mapping : ClassMap<ScheduleFormat>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Start);
            Map(s => s.End);
            Map(s => s.FacultyId);
            References(s => s.Faculty).Column(nameof(FacultyId))
                                      .ReadOnly()
                                      .LazyLoad();
        }
    }
}