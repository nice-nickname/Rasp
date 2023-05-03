using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteSubDisciplineKindCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        var subDisciplineKind = Repository.LoadById<SubDisciplineKind>(Id);
        Repository.Delete(subDisciplineKind);
    }
}
