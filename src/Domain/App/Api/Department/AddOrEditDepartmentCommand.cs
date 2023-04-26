using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.Api;

public class AddOrEditDepartmentCommand : CommandBase
{
    public int? Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public int FacultyId { get; set; }

    public class Validator : AbstractValidator<AddOrEditDepartmentCommand>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                    .NotEmpty().WithMessage(DataResources.InvalidEmpty);

            RuleFor(r => r.Code)
                    .NotEmpty().WithMessage(DataResources.InvalidEmpty);

            RuleFor(r => r.FacultyId)
                    .NotEmpty().WithMessage(DataResources.InvalidEmpty);
        }
    }

    protected override void Execute()
    {
        var department = Repository.GetById<Department>(Id.GetValueOrDefault()) ?? new Department();
        
        department.Code = Code;
        department.Name = Name;
        department.FacultyId = FacultyId;

        Repository.SaveOrUpdate(department);
    }

    public class AsQuery : QueryBase<AddOrEditDepartmentCommand>
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
