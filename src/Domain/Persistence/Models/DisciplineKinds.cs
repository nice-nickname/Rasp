using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class DisciplineKinds : IncEntityBase
{
    public virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public virtual string Code { get; set; }

    public class Mapping : ClassMap<DisciplineKinds>
    {
        public Mapping()
        {
            Table(nameof(DisciplineKinds));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
            Map(s => s.Code);
        }
    }
}
