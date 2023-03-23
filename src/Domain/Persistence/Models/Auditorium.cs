using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Auditorium : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Code { get; set; }

    public virtual int? DepartmentId { get; set; }

    public virtual int? BuildingId { get; set; }
 
    public virtual Department? Department { get; set; }

    public virtual Building? Building { get; set; }

    internal class Map : ClassMap<Auditorium>
    {
        public Map()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Code);
            Map(s => s.DepartmentId).Nullable();
            Map(s => s.BuildingId).Nullable();
            
            References(s => s.Building)
                .Column(nameof(BuildingId))
                .LazyLoad()
                .ReadOnly();
            
            References(s => s.Department)
                .Column(nameof(DepartmentId))
                .LazyLoad()
                .ReadOnly();
        }
    }
}