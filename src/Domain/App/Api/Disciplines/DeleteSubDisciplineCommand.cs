using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteSubDisciplineCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        var teachers = Repository.Query(new Share.Where.BySubDiscipline<SubDisciplineTeachers>(Id)).ToList();
        if (teachers.Any())
        {
            Repository.DeleteByIds<SubDisciplineTeachers>(teachers.Select(s => (object)s.Id));
        }

        var disciplinePlans = Repository.Query(new Share.Where.BySubDiscipline<DisciplinePlan>(Id)).ToList();
        if (disciplinePlans.Any())
        {
            Repository.DeleteByIds<DisciplinePlan>(disciplinePlans.Select(s => (object)s.Id));
        }

        var disciplineAuditoriumKinds = Repository.Query(new Share.Where.BySubDiscipline<SubDisciplineAuditoriumKinds>(Id)).ToList();
        if (disciplineAuditoriumKinds.Any())
        {
            Repository.DeleteByIds<SubDisciplineAuditoriumKinds>(disciplineAuditoriumKinds.Select(s => (object)s.Id));
        }

        var disciplineAuditoriums = Repository.Query(new Share.Where.BySubDiscipline<SubDisciplineAuditoriums>(Id)).ToList();
        if (disciplineAuditoriumKinds.Any())
        {
            Repository.DeleteByIds<SubDisciplineAuditoriums>(disciplineAuditoriums.Select(s => (object)s.Id));
        }

        Repository.Delete<SubDiscipline>(Id);
    }
}
