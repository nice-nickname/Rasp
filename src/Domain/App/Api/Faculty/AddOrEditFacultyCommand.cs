using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Resources;

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

        Dispatcher.Push(new AddOrEditFacultySettingCommand<DateTime>.IfNotExist
        {
                FacultyId = faculty.Id,
                Type = FacultySettings.OfType.StartDate,
                Value = new DateTime(1, 1, 1996)
        });
        Dispatcher.Push(new AddOrEditFacultySettingCommand<int>.IfNotExist
        {
                FacultyId = faculty.Id,
                Type = FacultySettings.OfType.CountOfWeeks,
                Value = 18
        });
        Dispatcher.Push(new AddOrEditFacultySettingCommand<int>.IfNotExist
        {
                FacultyId = faculty.Id,
                Type = FacultySettings.OfType.SessionStartWeek,
                Value = 16
        });
        Dispatcher.Push(new AddOrEditFacultySettingCommand<int>.IfNotExist
        {
                FacultyId = faculty.Id,
                Type = FacultySettings.OfType.SessionDurationInWeeks,
                Value = 3
        });
    }

    public class AsQuery : QueryBase<AddOrEditFacultyCommand>
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

    public class Validator : AbstractValidator<AddOrEditFacultyCommand>
    {
        public Validator()
        {
            RuleFor(r => r.Code)
                    .NotEmpty().WithMessage(DataResources.InvalidFacultyCode)
                    .NotNull().WithMessage(DataResources.InvalidFacultyCode);

            RuleFor(r => r.Name)
                    .NotEmpty().WithMessage(DataResources.InvalidFacultyName)
                    .NotNull().WithMessage(DataResources.InvalidFacultyName);
        }
    }
}
