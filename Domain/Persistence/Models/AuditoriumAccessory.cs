using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain;

public class AuditoriumAccessory : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual long Kinds { get; set; }

    public class Map : ClassMap<AuditoriumAccessory>
    {
        public Map()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Kinds);
        }
    }
}