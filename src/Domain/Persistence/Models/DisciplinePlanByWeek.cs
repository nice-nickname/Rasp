using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class DisciplinePlanByWeek : IncEntityBase, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual int AssignmentHours { get; set; }

    public virtual int Week { get; set; }

    public virtual int DisciplinePlanId { get; set; }

    public virtual DisciplinePlan DisciplinePlan { get; set; }

    public class Mapping : ClassMap<DisciplinePlanByWeek>
    {
        public Mapping()
        {
            Table(nameof(DisciplinePlanByWeek));
            Id(s => s.Id);
            Map(s => s.AssignmentHours);
            Map(s => s.Week);
            Map(s => s.DisciplinePlanId);

            References(s => s.DisciplinePlan).Column(nameof(DisciplinePlanId))
                                             .ReadOnly()
                                             .LazyLoad();
        }
    }
}
