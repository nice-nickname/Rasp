using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain;

public class Auditorium : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Code { get; set; }

    public virtual int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual AuditoriumAccessory? Accessories { get; set; }

    public class Map : ClassMap<Auditorium>
    {
        public Map()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Code);
            Map(s => s.DepartmentId).Nullable();

            References(s => s.Department).Column(nameof(DepartmentId))
                                         .Nullable()
                                         .Not.Insert()
                                         .Not.Update()
                                         .LazyLoad();

            References(s => s.Accessories).Nullable()
                                          .Not.Insert()
                                          .Not.Update();
        }
    }
}