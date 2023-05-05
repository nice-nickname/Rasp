using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class AuditoriumKind : IncEntityBase, Share.IEntityHasId
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
