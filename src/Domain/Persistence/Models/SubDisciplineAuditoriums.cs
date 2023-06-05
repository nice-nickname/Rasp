using System.Diagnostics.CodeAnalysis;
using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class SubDisciplineAuditoriums : IncEntityBase, Share.IEntityHasSubDiscipline
{
    public new virtual int Id { get; set; }

    public virtual int AuditoriumId { get; set; }

    public virtual int SubDisciplineId { get; set; }

    public virtual SubDiscipline SubDiscipline { get; set; }

    [ExcludeFromCodeCoverage]
    public class Mapping : ClassMap<SubDisciplineAuditoriums>
    {
        public Mapping()
        {
            Table(nameof(SubDisciplineAuditoriums));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.AuditoriumId);
            Map(s => s.SubDisciplineId);

            References(s => s.SubDiscipline).Column(nameof(SubDisciplineId))
                                            .ReadOnly()
                                            .LazyLoad();
        }
    }
}
