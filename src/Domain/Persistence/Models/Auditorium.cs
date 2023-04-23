using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class Auditorium : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual string Code { get; set; }

    public virtual int? DepartmentId { get; set; }

    public virtual int BuildingId { get; set; }

    public virtual int Capacity { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Building Building { get; set; }

    public virtual IList<AuditoriumKind> Kinds { get; set; }

    public Auditorium()
    {
        Kinds = new List<AuditoriumKind>();
    }

    public class Mapping : ClassMap<Auditorium>
    {
        public Mapping()
        {
            Table(nameof(Auditorium));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.DepartmentId).Nullable();
            Map(s => s.BuildingId);
            Map(s => s.Code);
            Map(s => s.Capacity);

            References(s => s.Department).Column(nameof(DepartmentId))
                                         .ReadOnly()
                                         .LazyLoad();

            References(s => s.Building).Column(nameof(BuildingId))
                                       .ReadOnly()
                                       .LazyLoad();

            HasManyToMany(s => s.Kinds)
                .Table(nameof(AuditoriumToKinds))
                .ParentKeyColumn(nameof(AuditoriumToKinds.AuditoriumId))
                .ChildKeyColumn(nameof(AuditoriumToKinds.AuditoriumKindId))
                .ReadOnly()
                .LazyLoad();
        }
    }
}
