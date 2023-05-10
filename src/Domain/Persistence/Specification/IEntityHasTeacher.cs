using System.Linq.Expressions;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence.Specification;

public partial class Share
{
    public interface IEntityHasTeacher
    {
        public int TeacherId { get; set; }

        public Teacher Teacher { get; set; }
    }

    public partial class Where
    {
        public class ByTeacher<T> : Specification<T> where T : IEntityHasTeacher
        {
            private readonly int _teacherId;

            public ByTeacher(int teacherId)
            {
                this._teacherId = teacherId;
            }

            public override Expression<Func<T, bool>> IsSatisfiedBy()
            {
                return s => s.TeacherId == _teacherId;
            }
        }
    }
}
