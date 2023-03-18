using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Auditorium : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Code { get; set; }

    public virtual int? DepartmentId { get; set; }

    public virtual int? AccessoriesId { get; set; }

    public virtual int? BuildingId { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Department? Department { get; set; }

    public virtual AuditoriumAccessory? Accessories { get; set; }

    internal class Map : ClassMap<Auditorium>
    {
        public Map()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Code);
            Map(s => s.DepartmentId).Nullable();
            Map(s => s.AccessoriesId).Nullable();
            Map(s => s.BuildingId).Nullable();

            References(s => s.Department).Column(nameof(DepartmentId))
                                         .Nullable()
                                         .Not.Insert()
                                         .Not.Update()
                                         .LazyLoad();

            References(s => s.Accessories).Column(nameof(AccessoriesId))
                                          .Nullable()
                                          .Not.Insert()
                                          .Not.Update()
                                          .LazyLoad();

            References(s => s.Building).Column(nameof(BuildingId))
                                       .Nullable()
                                       .Not.Insert()
                                       .Not.Update()
                                       .LazyLoad();
        }
    }
}