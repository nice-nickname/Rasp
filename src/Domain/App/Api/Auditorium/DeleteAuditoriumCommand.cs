using Incoding.Core.CQRS.Core;

namespace Domain.Api.Auditorium;

public class DeleteAuditoriumCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete<Persistence.Auditorium>(Id);
    }
}
