using Incoding.Core.CQRS.Core;
using Domain.Persistence;

namespace Domain.Api;

public class PrepareFacultyIfNotExistCommand : CommandBase
{
    protected override void Execute()
    {
        var facultyCount = Repository.Query<Faculty>()
                                     .Count();
        if (facultyCount == 0)
        {
            Repository.Save(new Faculty
            {
                Name = "Большой Армянский Биробиджанский Абсолютный Харам",
                Code = "БАБАХ"
            });
        }
    }
}