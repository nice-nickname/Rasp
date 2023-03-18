using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class AuditoriumAccessoriesKind : IncEntityBase
{
    public new virtual long Id { get; set; }

    public virtual string Name { get; set; }

    internal class Map : ClassMap<AuditoriumAccessoriesKind>
    {
        public Map()
        {
            Id(s => s.Id).GeneratedBy.Assigned();
            Map(s => s.Name);
        }
    }
}