using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class SaveFacultiesCommand : CommandBase
{
    public List<Item> Items { get; set; }

    protected override void Execute()
    {
        if (Items.Any(s => s.IsDeleted))
        {
            Repository.DeleteByIds<Faculty>(Items.Where(s => s.IsDeleted && s.Id.HasValue)
                                                 .Select(s => s.Id.GetValueOrDefault())
                                                 .Cast<object>());
        }

        foreach (var faculty in Items.Where(s => !s.IsDeleted))
        {
            Dispatcher.Push(new AddOrEditFacultyCommand
            {
                Id = faculty.Id,
                Code = faculty.Code,
                Name = faculty.Name,
            });
        }
    }

    public class Validator : AbstractValidator<SaveFacultiesCommand>
    {
        public Validator()
        {
            RuleForEach(s => s.Items).SetValidator(new ItemValidator());
        }
    }

    public class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator()
        {
            RuleFor(s => s.Name).NotNull().NotEmpty();
            RuleFor(s => s.Code).NotNull().NotEmpty();
        }
    }

    public record Item
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsDeleted { get; set; }
    }
}
