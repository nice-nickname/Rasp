using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class ResetAllWeekendsCommand : CommandBase
{
    protected override void Execute()
    {
        Repository.DeleteAll<Holidays>();
    }
}
