using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class SubDisciplineAuditoriumKinds : IncEntityBase, Share.IEntityHasSubDiscipline
{
    public new virtual int Id { get; set; }

    public virtual int AuditoriumKindId { get; set; }

    public virtual int SubDisciplineId { get; set; }

    public virtual SubDiscipline SubDiscipline { get; set; }

    public class Mapping : ClassMap<SubDisciplineAuditoriumKinds>
    {
        public Mapping()
        {
            Table(nameof(SubDisciplineAuditoriumKinds));
            Id(s => s.Id);
            Map(s => s.AuditoriumKindId);
            Map(s => s.SubDisciplineId);

            References(s => s.SubDiscipline).Column(nameof(SubDisciplineId))
                                            .ReadOnly()
                                            .LazyLoad();
        }
    }
}
