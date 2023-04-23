using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence;

public class DisciplineGroups : IncEntityBase
{
    public new virtual int Id { get; set; }

    public virtual int GroupId { get; set; }

    public virtual int DisciplineId { get; set; }

    public class Mapping : ClassMap<DisciplineGroups>
    {
        public Mapping()
        {
            Table(nameof(DisciplineGroups));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.DisciplineId);
            Map(s => s.GroupId);
        }
    }

    public class Where
    {
        public class ByDiscipline : Specification<DisciplineGroups>
        {
            private readonly int _disciplineId;

            public ByDiscipline(int disciplineId)
            {
                this._disciplineId = disciplineId;
            }

            public override Expression<Func<DisciplineGroups, bool>> IsSatisfiedBy()
            {
                return s => s.DisciplineId == this._disciplineId;
            }
        }
    }
}
