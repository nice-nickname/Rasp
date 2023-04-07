using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class AuditoriumToKinds : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int AuditoriumId { get; set; }

    public virtual int AuditoriumKindId { get; set; }

    public class Mapping : ClassMap<AuditoriumToKinds>
    {
        public Mapping()
        {
            Table(nameof(AuditoriumToKinds));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.AuditoriumId);
            Map(s => s.AuditoriumKindId);
        }
    }
}

public class AuditoriumKind : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Kind { get; set; }

    public class Mapping : ClassMap<AuditoriumKind>
    {
        public Mapping()
        {
            Table(nameof(AuditoriumKind));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Kind);
        }
    }
}