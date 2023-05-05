using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Holidays : IncEntityBase, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual DateOnly Date { get; set; }

    public virtual string? Name { get; set; }

    public class Mapping : ClassMap<Holidays>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name).Nullable();
            Map(s => s.Date).CustomType<Mappers.NHibernateDateOnlyType>();
        }
    }
}
