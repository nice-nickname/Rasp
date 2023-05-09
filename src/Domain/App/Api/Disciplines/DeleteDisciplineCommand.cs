using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteDisciplineCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        var discipline = Repository.GetById<Discipline>(Id);

        var groups = Repository.Query(new Share.Where.ByDiscipline<DisciplineGroups>(discipline.Id))
                               .ToList();
        if (groups.Any())
        {
            Repository.DeleteByIds<DisciplineGroups>(groups.Select(s => (object)s.Id));
        }

        foreach (var subDiscipline in discipline.SubDisciplines)
        {
            Dispatcher.Push(new DeleteSubDisciplineCommand
            {
                    Id = subDiscipline.Id
            });
        }
        
        Repository.Delete<Discipline>(discipline.Id);
    }
}
