using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class DisciplinePlan : IncEntityBase, Share.IEntityHasSubDiscipline
{
    public new virtual int Id { get; set; }

    public virtual int SubDisciplineId { get; set; }

    public virtual int TeacherId { get; set; }

    public virtual int GroupId { get; set; }

    public virtual int SubGroupsCount { get; set; }

    public virtual SubDiscipline SubDiscipline { get; set; }

    public virtual Teacher Teacher { get; set; }

    public virtual Group Group { get; set; }

    public class Mapping : ClassMap<DisciplinePlan>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.SubGroupsCount);
            Map(s => s.SubDisciplineId);
            Map(s => s.TeacherId);
            Map(s => s.GroupId);

            References(s => s.SubDiscipline).Column(nameof(SubDisciplineId))
                                            .ReadOnly()
                                            .LazyLoad();

            References(s => s.Teacher).Column(nameof(TeacherId))
                                      .ReadOnly()
                                      .LazyLoad();

            References(s => s.Group).Column(nameof(GroupId))
                                    .ReadOnly()
                                    .LazyLoad();
        }
    }
}
