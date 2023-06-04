using Incoding.Core.CQRS.Core;
using Incoding.UnitTests.MSpec;

namespace Domain.Test;

class query_spec<TQuery, TResponse> where TQuery : QueryBase<TResponse>
{
    protected static TQuery query;

    protected static MockMessage<TQuery, TResponse> mockQuery;

    protected static Action<TQuery> mock;
}
