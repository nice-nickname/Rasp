using System.Linq.Expressions;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence.Specification;

public partial class Share
{
    public interface IEntityHasSubDiscipline
    {
        int SubDisciplineId { get; }

        SubDiscipline SubDiscipline { get; }
    }

    public partial class Where
    {
        public class BySubDiscipline<T> : Specification<T> where T : IEntityHasSubDiscipline
        {
            private readonly int _subDisciplineId;

            public BySubDiscipline(int subDisciplineId)
            {
                this._subDisciplineId = subDisciplineId;
            }

            public override Expression<Func<T, bool>> IsSatisfiedBy()
            {
                return s => s.SubDisciplineId == _subDisciplineId;
            }
        }
    }
}
