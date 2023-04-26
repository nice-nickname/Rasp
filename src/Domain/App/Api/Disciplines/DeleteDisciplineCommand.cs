using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteDisciplineCommand : CommandBase
{
    public int DisciplineId { get; set; }

    protected override void Execute()
    {
        
    }
}
