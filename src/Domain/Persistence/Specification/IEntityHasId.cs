using System.Linq.Expressions;
using Incoding.Core.Data;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence.Specification;

public partial class Share
{
    public interface IEntityHasId
    {
        int Id { get; }
    }

    public class ById<T> : Specification<T> where T : IEntityHasId
    {
        private readonly int _id;

        public ById(int id)
        {
            this._id = id;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            return s => s.Id == this._id;
        }
    }

    public class HasId<T> : Specification<T> where T : IEntityHasId
    {
        private readonly int[] _ids;

        public HasId(int[] ids)
        {
            this._ids = ids;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            return s => this._ids.Contains(s.Id);
        }
    }
}
