using Incoding.Core.CQRS.Core;

namespace Domain.Api.AuditoriumKind;

public class DeleteAuditoriumKindCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete<Persistence.AuditoriumKind>(Id);
    }
}
