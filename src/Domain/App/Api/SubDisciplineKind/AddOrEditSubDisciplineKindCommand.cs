using System.Drawing;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class AddOrEditSubDisciplineKindCommand : CommandBase
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public Color Color { get; set; }

    public SubDisciplineKind.OfType Type { get; set; }

    protected override void Execute()
    {
        var disciplineKind = Repository.GetById<SubDisciplineKind>(Id) ?? new SubDisciplineKind();
        disciplineKind.Name = Name;
        disciplineKind.Code = Code;
        disciplineKind.Color = Color;
        disciplineKind.Type = Type;
        Repository.SaveOrUpdate(disciplineKind);
    }

    public class AsQuery : QueryBase<AddOrEditSubDisciplineKindCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditSubDisciplineKindCommand ExecuteResult()
        {
            var kind = Repository.GetById<SubDisciplineKind>(Id);
            return new AddOrEditSubDisciplineKindCommand
            {
                    Id = kind.Id,
                    Name = kind.Name,
                    Code = kind.Code,
                    Color = kind.Color,
                    Type = kind.Type
            };
        }
    }
}
