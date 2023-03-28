using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Faculty : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Name { get; set; }

    public class Mapping : ClassMap<Faculty>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name);
        }
    }
}