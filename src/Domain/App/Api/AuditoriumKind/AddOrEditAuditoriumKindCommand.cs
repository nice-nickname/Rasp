using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class AddOrEditAuditoriumKindCommand : CommandBase
{
    public int? Id { get; set; }

    public string Kind { get; set; }

    protected override void Execute()
    {
        var auditoriumKind = Repository.GetById<AuditoriumKind>(Id.GetValueOrDefault()) ?? new AuditoriumKind();

        auditoriumKind.Kind = Kind;

        Repository.SaveOrUpdate(auditoriumKind);
    }

    public class AsQuery : QueryBase<AddOrEditAuditoriumKindCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditAuditoriumKindCommand ExecuteResult()
        {
            var auditoriumKind = Repository.GetById<AuditoriumKind>(Id.GetValueOrDefault()) ?? new AuditoriumKind();

            return new AddOrEditAuditoriumKindCommand
            {
                    Id = auditoriumKind.Id,
                    Kind = auditoriumKind.Kind
            };
        }
    }
}
