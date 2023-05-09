using Domain.Persistence;
using Domain.Persistence.Specification;
using FluentNHibernate.Utils;
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

            var disciplinePlans = Repository.Query(new Share.Where.BySubDiscipline<DisciplinePlan>(subDiscipline.Id)).ToList();
            if (disciplinePlans.Any())
            {
                Repository.DeleteByIds<DisciplinePlan>(disciplinePlans.Select(s => (object)s.Id));
            }
        }

        foreach (var subDiscipline in discipline.SubDisciplines)
        {
            var disciplineAuditoriumKinds = Repository.Query(new Share.Where.BySubDiscipline<SubDisciplineAuditoriumKinds>(subDiscipline.Id)).ToList();
            if (disciplineAuditoriumKinds.Any())
            {
                Repository.DeleteByIds<SubDisciplineAuditoriumKinds>(disciplineAuditoriumKinds.Select(s => (object)s.Id));
            }

            var disciplineAuditoriums = Repository.Query(new Share.Where.BySubDiscipline<SubDisciplineAuditoriums>(subDiscipline.Id)).ToList();
            if (disciplineAuditoriumKinds.Any())
            {
                Repository.DeleteByIds<SubDisciplineAuditoriums>(disciplineAuditoriums.Select(s => (object)s.Id));
            }
        }

        if (discipline.SubDisciplines.Any())
        {
            Repository.DeleteByIds<SubDiscipline>(discipline.SubDisciplines.Select(s => (object)s.Id));
        }
        Repository.DeleteByIds<Discipline>(new List<object>(){ discipline.Id });
    }
}
