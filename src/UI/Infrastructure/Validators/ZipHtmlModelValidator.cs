using Domain.Common;
using FluentValidation;

namespace UI.Infrastructure.Validators;

public class ZipHtmlModelValidator : AbstractValidator<ZipHtmlModel>
{
    public ZipHtmlModelValidator()
    {
        RuleFor(s => s.StartWeek).Must((model, _) => model.StartWeek <= model.EndWeek).WithMessage("Некорректная неделя");
        RuleFor(s => s.Groups).Must((zip, _) => (zip.Auditoriums != null && zip.Auditoriums.Any()) ||
                                                (zip.Teachers != null && zip.Teachers.Any()) ||
                                                (zip.Groups != null && zip.Groups.Any()))
                              .WithMessage("Выберите хотя бы один предмет для экспорта");
    }
}
