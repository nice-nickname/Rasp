using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteSubDisciplineKindCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        var relatedSubDisciplines = Repository.Query<SubDiscipline>()
                                              .Where(s => s.Kind.Id == Id)
                                              .ToList();

        foreach (var subDiscipline in relatedSubDisciplines.Select(s => s.Id))
        {
            Dispatcher.Push(new DeleteSubDisciplineCommand
            {
                    Id = subDiscipline
            });
        }

        Repository.Delete<SubDisciplineKind>(Id);
    }
}
