using System.Drawing;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class SubDisciplineKind : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual string Code { get; set; }

    public virtual Color Color { get; set; }

    public class Mapping : ClassMap<SubDisciplineKind>
    {
        public Mapping()
        {
            Table(nameof(SubDisciplineKind));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            Map(s => s.Code);
            Map(s => s.Color).CustomType<Mappers.NHibernateColorType>();
        }
    }
}
