using System.Linq.Expressions;
using Domain.Persistence.Specification;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence;

public class Holidays : IncEntityBase, Share.IEntityHasId
{
    public new virtual int Id { get; set; }

    public virtual DateOnly Date { get; set; }

    public virtual string? Name { get; set; }

    public class Mapping : ClassMap<Holidays>
    {
        public Mapping()
        {
            Table(nameof(Holidays));
            Id(s => s.Id).GeneratedBy.Identity();
            Map(s => s.Name).Nullable();
            Map(s => s.Date).CustomType<Mappers.NHibernateDateOnlyType>();
        }
    }

    public class Where
    {
        public class BetweenDates : Specification<Holidays>
        {
            private readonly DateOnly start;

            private readonly DateOnly end;

            public BetweenDates(DateTime start, DateTime end)
            {
                this.start = DateOnly.FromDateTime(start);
                this.end = DateOnly.FromDateTime(end);
            }

            public override Expression<Func<Holidays, bool>> IsSatisfiedBy()
            {
                return s => s.Date >= this.start && s.Date <= this.end;
            }
        }
    }
}
