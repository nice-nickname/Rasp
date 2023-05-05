using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class DisciplineKind : IncEntityBase, Share.IEntityHasId
{
    public virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public class Mapping : ClassMap<DisciplineKind>
    {
        public Mapping()
        {
            Table(nameof(DisciplineKind));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
        }
    }
}
