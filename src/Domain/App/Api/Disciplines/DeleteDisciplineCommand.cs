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
            var teachers = Repository.Query(new Share.Where.BySubDiscipline<SubDisciplineTeachers>(subDiscipline.Id)).ToList();
            if (teachers.Any())
            {
                Repository.DeleteByIds<SubDisciplineTeachers>(teachers.Select(s => (object)s.Id));
            }
        }

        if (discipline.SubDisciplines.Any())
        {
            Repository.DeleteByIds<SubDiscipline>(discipline.SubDisciplines.Select(s => (object)s.Id));
        }
        Repository.DeleteByIds<Discipline>(new List<object>(){ discipline.Id });
    }
}
