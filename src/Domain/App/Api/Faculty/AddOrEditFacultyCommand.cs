namespace Domain.Api;

using Domain.Persistence;
using Incoding.Core.CQRS.Core;

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

    public class AsView : QueryBase<AddOrEditFacultyCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditFacultyCommand ExecuteResult()
        {
            var faculty = Repository.GetById<Faculty>(Id.GetValueOrDefault()) ?? new Faculty();

            return new AddOrEditFacultyCommand
            {
                    Id = faculty.Id,
                    Code = faculty.Code,
                    Name = faculty.Name
            };
        }
    }
}
