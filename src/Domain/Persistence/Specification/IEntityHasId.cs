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

    public class ById : Specification<IEntityHasId>
    {
        private readonly int _id;

        public ById(int id)
        {
            this._id = id;
        }

        public override Expression<Func<IEntityHasId, bool>> IsSatisfiedBy()
        {
            return s => s.Id == this._id;
        }
    }

    public class HasId : Specification<IEntityHasId>
    {
        private readonly int[] _ids;

        public HasId(int[] ids)
        {
            this._ids = ids;
        }

        public override Expression<Func<IEntityHasId, bool>> IsSatisfiedBy()
        {
            return s => this._ids.Contains(s.Id);
        }
    }
}
