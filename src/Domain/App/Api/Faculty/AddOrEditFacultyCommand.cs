using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class AddOrEditFacultyCommand : CommandBase
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    protected override void Execute()
    {
        var faculty = Repository.GetById<Faculty>(Id.GetValueOrDefault()) ?? new Faculty();
        faculty.Name = Name;
        faculty.Code = Code;
        Repository.SaveOrUpdate(faculty);
    }
}