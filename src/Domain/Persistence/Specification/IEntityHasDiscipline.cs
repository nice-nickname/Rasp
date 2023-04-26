using System.Linq.Expressions;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence.Specification;

public partial class Share
{
    public interface IEntityHasDiscipline
    {
        int DisciplineId { get; set; }

        Discipline Discipline { get; set; }
    }

    public partial class Where
    {
        public class ByDiscipline<T> : Specification<T>  where T : IEntityHasDiscipline
        {
            private readonly int _disciplineId;

            public ByDiscipline(int id)
            {
                this._disciplineId = id;
            }

            public override Expression<Func<T, bool>> IsSatisfiedBy()
            {
                return s => s.DisciplineId == this._disciplineId;
            }
        }
    }
}
