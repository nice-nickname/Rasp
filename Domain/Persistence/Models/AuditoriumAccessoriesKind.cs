using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain;

public class AuditoriumAccessoriesKind : IncEntityBase
{
    public new virtual long Id { get; set; }

    public virtual string Name { get; set; }

    public class Map : ClassMap<AuditoriumAccessoriesKind>
    {
        public Map()
        {
            Id(s => s.Id).GeneratedBy.Assigned();
            Map(s => s.Name);
        }
    }
}