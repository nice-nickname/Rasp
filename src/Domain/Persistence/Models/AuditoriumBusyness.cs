using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class AuditoriumBusyness : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Reason { get; set; }

    public virtual DateTime Start { get; set; }

    public virtual DateTime End { get; set; }

    public virtual int AuditoriumId { get; set; }

    public class Mapping : ClassMap<AuditoriumBusyness>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Reason);
            Map(s => s.Start);
            Map(s => s.End);
            Map(s => s.AuditoriumId);
        }
    }
}
