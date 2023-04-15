namespace Domain.Api;

using Domain.Persistence;
using Incoding.Core.CQRS.Core;

public class AddOrEditDepartmentCommand : CommandBase
{
    public int? Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public int FacultyId { get; set; }

    protected override void Execute()
    {
        var department = Repository.GetById<Department>(Id.GetValueOrDefault()) ?? new Department();
        department.Code = Code;
        department.Name = Name;
        department.FacultyId = FacultyId;
        Repository.SaveOrUpdate(department);
    }

    public class AsView : QueryBase<AddOrEditDepartmentCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditDepartmentCommand ExecuteResult()
        {
            var department = Repository.GetById<Department>(Id.GetValueOrDefault()) ?? new Department();

            return new AddOrEditDepartmentCommand
            {
                    Id = department.Id,
                    Code = department.Code,
                    Name = department.Name,
                    FacultyId = department.FacultyId
            };
        }
    }
}
