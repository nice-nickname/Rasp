using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteClassCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete<Class>(Id);
    }
}
