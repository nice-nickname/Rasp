using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.Api;

public class AddOrEditGroupCommand : CommandBase
{
    public int? Id { get; set; }

    public int DepartmentId { get; set; }

    public int StudentCount { get; set; }

    public string Code { get; set; }

    public int Course { get; set; }

    protected override void Execute()
    {
        var group = Repository.GetById<Group>(Id) ?? new Group();

        group.Code = Code;
        group.DepartmentId = DepartmentId;
        group.StudentCount = StudentCount;
        group.Course = Course;

        Repository.SaveOrUpdate(group);
    }

    public class AsQuery : QueryBase<AddOrEditGroupCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditGroupCommand ExecuteResult()
        {
            var group = Repository.GetById<Group>(Id) ?? new Group();

            return new AddOrEditGroupCommand
            {
                    Id = group.Id,
                    Code = group.Code,
                    DepartmentId = group.DepartmentId,
                    StudentCount = group.StudentCount,
                    Course = group.Course,
                    
            };
        }
    }

    public class Validator : AbstractValidator<AddOrEditGroupCommand>
    {
        public Validator()
        {
            RuleFor(s => s.Code).NotEmpty().NotNull().WithName(DataResources.GroupCode);
            RuleFor(s => s.Course).InclusiveBetween(1, 5).WithName(DataResources.Course);
            RuleFor(s => s.StudentCount).GreaterThan(0).WithName(DataResources.StudentCount);
        }
    }
}
