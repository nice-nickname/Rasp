using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class SubDisciplineTeachers : IncEntityBase, Share.IEntityHasSubDiscipline
{
    public new virtual int Id { get; set; }

    public virtual int TeacherId { get; set; }

    public virtual int SubDisciplineId { get; set; }

    public virtual SubDiscipline SubDiscipline { get; set; }

    public class Mapping : ClassMap<SubDisciplineTeachers>
    {
        public Mapping()
        {
            Table(nameof(SubDisciplineTeachers));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.SubDisciplineId);
            Map(s => s.TeacherId);

            References(s => s.SubDiscipline).Column(nameof(SubDisciplineId))
                                            .ReadOnly()
                                            .LazyLoad();
        }
    }
}
