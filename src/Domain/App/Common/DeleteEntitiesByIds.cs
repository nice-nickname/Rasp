using Incoding.Core.CQRS.Core;
using Domain.Persistence.Specification;
using Incoding.Core.Data;

namespace Domain;

public class DeleteEntitiesByIds<T> : CommandBase where T : IncEntityBase, Share.IEntityHasId
{
    private readonly IEnumerable<object> _ids;

    public DeleteEntitiesByIds(IEnumerable<int> ids)
    {
        this._ids = ids.Cast<object>();
    }

    public DeleteEntitiesByIds(params int[] ids)
    {
        this._ids = ids.Cast<object>();
    }
    
    protected override void Execute()
    {
        if (!_ids.Any())
            return;
        
        Repository.ExecuteSql($"DELETE FROM [{typeof(T).Name}] WHERE [Id] IN ({string.Join(',', _ids)})");
    }
}
