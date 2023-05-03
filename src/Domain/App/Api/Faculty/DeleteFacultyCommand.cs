using Domain.Persistence;
using Domain.Persistence.Specification;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class DeleteFacultyCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        var facultySettingsIds = Repository.Query(new Share.Where.ByFaculty<FacultySettings>(Id))
                                        .Select(s => (object)s.Id)
                                        .ToList();

        if (facultySettingsIds.Any())
        {
            Repository.DeleteByIds<FacultySettings>(facultySettingsIds);
        }

        var scheduleFormatIds = Repository.Query(new Share.Where.ByFaculty<ScheduleFormat>(Id))
                                          .Select(s => (object)s.Id)
                                          .ToList();

        if (scheduleFormatIds.Any())
        {
            Repository.DeleteByIds<ScheduleFormat>(scheduleFormatIds);
        }

        foreach (var department in Repository.Query(new Share.Where.ByFaculty<Department>(Id)))
        {
            Dispatcher.Push(new DeleteDepartmentCommand
            {
                    Id = department.Id,
            });
        }

        Repository.Delete<Faculty>(Id);
    }
}
