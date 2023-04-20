using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class PrepareFacultyIfNotExistCommand : CommandBase
{
    protected override void Execute()
    {
        if (!Repository.Query<Faculty>().Any())
        {
            Repository.Save(new Faculty
            {
                    Name = "Институт компьютерных технологий и информационной безопасности",
                    Code = "ИКТИБ"
            });
        }
    }
}
