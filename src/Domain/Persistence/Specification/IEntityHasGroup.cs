﻿using System.Linq.Expressions;
using Incoding.Core.Extensions.LinqSpecs;

namespace Domain.Persistence.Specification;

public partial class Share
{
    public interface IEntityHasGroup
    {
        public int GroupId { get; }

        public Group Group { get; }
    }

    public partial class Where
    {
        public class ByGroup : Specification<IEntityHasGroup>
        {
            private readonly int _groupId;

            public ByGroup(int groupId)
            {
                this._groupId = groupId;
            }

            public override Expression<Func<IEntityHasGroup, bool>> IsSatisfiedBy()
            {
                return s => s.GroupId == _groupId;
            }
        }

        public class HasGroup : Specification<IEntityHasGroup>
        {
            private readonly int[] _groupIds;

            public HasGroup(int[] groupIds)
            {
                this._groupIds = groupIds;
            }

            public override Expression<Func<IEntityHasGroup, bool>> IsSatisfiedBy()
            {
                return s => this._groupIds.Contains(s.GroupId);
            }
        }
    }
}
