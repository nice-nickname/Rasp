using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteDepartmentCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        foreach (var disciplineId in Repository.Query(new Share.Where.ByNullableDepartment<Discipline>(Id)).Select(s => s.Id))
        {
            Dispatcher.Push(new DeleteDisciplineCommand { Id = disciplineId });
        }

        var groups = Repository.Query(new Share.Where.ByDepartment<Group>(Id))
                               .Select(s => s.Id)
                               .ToArray();
        
        var teachers = Repository.Query(new Share.Where.ByDepartment<Teacher>(Id))
                                 .Select(s => s.Id)
                                 .ToArray();

        var disciplineGroups = Repository.Query<DisciplineGroups>()
                                         .Where(s => groups.Contains(s.GroupId))
                                         .Select(s => s.Id)
                                         .ToList();

        var subDisciplineTeachers = Repository.Query<SubDisciplineTeachers>()
                                              .Where(s => teachers.Contains(s.TeacherId))
                                              .Select(s => s.Id)
                                              .ToList();

        Dispatcher.Push(new DeleteEntitiesByIds<DisciplineGroups>(disciplineGroups));
        Dispatcher.Push(new DeleteEntitiesByIds<SubDisciplineTeachers>(subDisciplineTeachers));

        var disciplinePlans = Repository.Query<DisciplinePlan>()
                                        .Where(s => teachers.Contains(s.TeacherId) || groups.Contains(s.GroupId))
                                        .Select(s => s.Id)
                                        .ToList();

        Dispatcher.Push(new DeleteEntitiesByIds<DisciplinePlan>(disciplinePlans));
        Dispatcher.Push(new DeleteEntitiesByIds<Group>(groups));
        Dispatcher.Push(new DeleteEntitiesByIds<Teacher>(teachers));
        
        Repository.Delete<Department>(Id);
    }
}
