using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.Api;

public class AddOrEditTeacherCommand : CommandBase
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public int DepartmentId { get; set; }

    protected override void Execute()
    {
        var teacher = Repository.GetById<Teacher>(Id) ?? new Teacher();

        teacher.Name = Name;
        teacher.DepartmentId = DepartmentId;

        Repository.SaveOrUpdate(teacher);
    }

    public class AsView : QueryBase<AddOrEditTeacherCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditTeacherCommand ExecuteResult()
        {
            var teacher = Repository.GetById<Teacher>(Id) ?? new Teacher();

            return new AddOrEditTeacherCommand
            {
                    Id = teacher.Id,
                    Name = teacher.Name,
                    DepartmentId = teacher.DepartmentId
            };
        }
    }

    public class Validator : AbstractValidator<AddOrEditTeacherCommand>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                    .Must(r => r.Split(" ").Length == 3)
                    .WithMessage(DataResources.InvalidTeacherName);
        }
    }
}
