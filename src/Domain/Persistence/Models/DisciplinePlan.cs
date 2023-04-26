using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class DisciplinePlan : IncEntityBase, Share.IEntityHasSubDiscipline
{
    public new virtual int Id { get; set; }

    public virtual int SubDisciplineId { get; set; }

    public virtual int TeacherId { get; set; }

    public virtual int Week { get; set; }

    public virtual int Hours { get; set;}

    public virtual SubDiscipline SubDiscipline { get; set; }

    public class Mapping : ClassMap<DisciplinePlan>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.SubDisciplineId);
            Map(s => s.TeacherId);
            Map(s => s.Week);
            Map(s => s.Hours);

            References(s => s.SubDiscipline).ForeignKey(nameof(SubDisciplineId))
                                            .ReadOnly()
                                            .LazyLoad();
        }
    }
}
