using Incoding.Core.CQRS.Core;

namespace Domain.Api.AuditoriumKind;

public class AddOrEditAuditoriumKindCommand : CommandBase
{
    public int? Id { get; set; }

    public string Kind { get; set; }

    protected override void Execute()
    {
        var auditoriumKind = Repository.GetById<Persistence.AuditoriumKind>(Id.GetValueOrDefault()) ?? new Persistence.AuditoriumKind();

        auditoriumKind.Kind = Kind;

        Repository.SaveOrUpdate(auditoriumKind);
    }

    public class AsView : QueryBase<AddOrEditAuditoriumKindCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditAuditoriumKindCommand ExecuteResult()
        {
            var auditoriumKind = Repository.GetById<Persistence.AuditoriumKind>(Id.GetValueOrDefault()) ?? new Persistence.AuditoriumKind();

            return new AddOrEditAuditoriumKindCommand
            {
                    Id = auditoriumKind.Id,
                    Kind = auditoriumKind.Kind
            };
        }
    }
}
