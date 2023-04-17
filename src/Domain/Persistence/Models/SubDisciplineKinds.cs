using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class SubDisciplineKinds : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual string Code { get; set; }

    public class Mapping : ClassMap<SubDisciplineKinds>
    {
        public Mapping()
        {
            Table(nameof(SubDisciplineKinds));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            Map(s => s.Code);
        }
    }
}
