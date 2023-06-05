using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Persistence;

public class DisciplineKind : IncEntityBase, Share.IEntityHasId
{
    public virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual SubDisciplineKind.OfType? Type { get; set; }

    [ExcludeFromCodeCoverage]
    public class Mapping : ClassMap<DisciplineKind>
    {
        public Mapping()
        {
            Table(nameof(DisciplineKind));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            Map(s => s.Type).CustomType<SubDisciplineKind.OfType>().Nullable();
        }
    }
}
