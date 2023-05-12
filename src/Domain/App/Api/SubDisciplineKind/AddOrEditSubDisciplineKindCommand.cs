using System.Drawing;
using Domain.Persistence;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Resources;

namespace Domain.Api;

public class AddOrEditSubDisciplineKindCommand : CommandBase
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string Color { get; set; }

    public SubDisciplineKind.OfType Type { get; set; }

    protected override void Execute()
    {
        var disciplineKind = Repository.GetById<SubDisciplineKind>(Id) ?? new SubDisciplineKind();
        disciplineKind.Name = Name;
        disciplineKind.Code = Code;
        disciplineKind.Color = ColorTranslator.FromHtml(Color);
        disciplineKind.Type = Type;
        Repository.SaveOrUpdate(disciplineKind);
    }

    public class AsQuery : QueryBase<AddOrEditSubDisciplineKindCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditSubDisciplineKindCommand ExecuteResult()
        {
            if (!Id.HasValue)
            {
                return new AddOrEditSubDisciplineKindCommand
                {
                    Name = string.Empty,
                    Code = string.Empty,
                    Color = ColorTranslator.ToHtml(System.Drawing.Color.Black),
                    Type = SubDisciplineKind.OfType.LECTURE
                };
            }
            var kind = Repository.GetById<SubDisciplineKind>(Id);
            return new AddOrEditSubDisciplineKindCommand
            {
                    Id = kind.Id,
                    Name = kind.Name,
                    Code = kind.Code,
                    Color = ColorTranslator.ToHtml(kind.Color),
                    Type = kind.Type
            };
        }
    }

    public class Validator : AbstractValidator<AddOrEditSubDisciplineKindCommand>
    {
        public Validator()
        {
            RuleFor(s => s.Name).NotEmpty().NotNull().WithName(DataResources.SubDisciplineName);
            RuleFor(s => s.Code).NotEmpty().NotNull().WithName(DataResources.Abbreviation);
        }
    }
}
