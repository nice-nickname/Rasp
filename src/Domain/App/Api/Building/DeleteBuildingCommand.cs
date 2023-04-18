using Incoding.Core.CQRS.Core;

namespace Domain.Api.Building;

public class DeleteBuildingCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete<Persistence.Building>(Id);
    }
}
