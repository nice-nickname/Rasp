using System.Diagnostics.CodeAnalysis;
using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class TeacherBusyness : IncEntityBase, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual string Reason { get; set; }

    public virtual DateTime Start { get; set; }

    public virtual DateTime End { get; set; }

    public virtual int TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; }

    [ExcludeFromCodeCoverage]
    public class Mapping : ClassMap<TeacherBusyness>
    {
        public Mapping()
        {
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Reason);
            Map(s => s.Start);
            Map(s => s.End);
            Map(s => s.TeacherId);

            References(s => s.Teacher).Column(nameof(TeacherId))
                                      .ReadOnly()
                                      .LazyLoad();
        }
    }
}
