using System.Diagnostics.CodeAnalysis;
using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class SubDisciplineTeachers : IncEntityBase, Share.IEntityHasSubDiscipline, Share.IEntityHasId, Share.IEntityHasTeacher
{
    public new virtual int Id { get; set; }

    public virtual int TeacherId { get; set; }

    public virtual int SubDisciplineId { get; set; }

    public virtual SubDiscipline SubDiscipline { get; set; }

    public virtual Teacher Teacher { get; set; }

    [ExcludeFromCodeCoverage]
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

            References(s => s.Teacher).Column(nameof(TeacherId))
                                      .ReadOnly()
                                      .LazyLoad();
        }
    }
}
