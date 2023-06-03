using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Persistence;

public class AuditoriumToKinds : IncEntityBase, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual int AuditoriumId { get; set; }

    public virtual int AuditoriumKindId { get; set; }

    [ExcludeFromCodeCoverage]
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
