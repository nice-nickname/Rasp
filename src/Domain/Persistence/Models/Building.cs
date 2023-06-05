using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Persistence;

public class Building : IncEntityBase, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    [ExcludeFromCodeCoverage]
    public class Mapping : ClassMap<Building>
    {
        public Mapping()
        {
            Table(nameof(Building));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
        }
    }
}
