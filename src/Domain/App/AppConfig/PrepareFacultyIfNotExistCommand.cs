using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class PrepareFacultyIfNotExistCommand : CommandBase
{
    protected override void Execute()
    {
        var faculties = Repository.Query<Faculty>();
        if (!faculties.Any())
        {
            Dispatcher.Push(new AddOrEditFacultyCommand
            {
                    Name = "Институт компьютерных технологий и информационной безопасности",
                    Code = "ИКТИБ"
            });
            return;
        }

        foreach (var faculty in faculties)
        {
            Dispatcher.Push(new AddOrEditFacultyCommand
            {
                    Name = faculty.Name,
                    Code = faculty.Code,
                    Id = faculty.Id
            });
        }
    }
}
