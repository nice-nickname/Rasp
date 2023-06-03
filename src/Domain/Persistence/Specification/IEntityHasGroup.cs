using System.Linq.Expressions;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence.Specification;

public partial class Share
{
    public interface IEntityHasGroup
    {
        public int GroupId { get; set; }

        public Group Group { get; set; }
    }

    public partial class Where
    {
        public class ByGroup<T> : Specification<T> where T : IEntityHasGroup
        {
            private readonly int _groupId;

            public ByGroup(int groupId)
            {
                this._groupId = groupId;
            }

            public override Expression<Func<T, bool>> IsSatisfiedBy()
            {
                return s => s.GroupId == _groupId;
            }
        }

        public class HasGroup<T> : Specification<T> where T : IEntityHasGroup
        {
            private readonly int[] _groupIds;

            public HasGroup(int[] groupIds)
            {
                this._groupIds = groupIds;
            }

            public override Expression<Func<T, bool>> IsSatisfiedBy()
            {
                return s => this._groupIds.Contains(s.GroupId);
            }
        }
    }
}

