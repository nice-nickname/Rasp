using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class DisciplinePlanByWeek : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int AssignmentHours { get; set; }

    public virtual int Week { get; set; }

    public virtual int DisciplinePlanId { get; set; }

    public virtual DisciplinePlanByWeek DisciplinePlan { get; set; }

    public class Mapping : ClassMap<DisciplinePlanByWeek>
    {
        public Mapping()
        {
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
