using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class AddOrEditAuditoriumCommand : CommandBase
{
    public int? Id { get; set; }

    public int? DepartmentId { get; set; }

    public int? BuildingId { get; set; }

    public string Code { get; set; }

    public List<AuditoriumKind> Kinds { get; set; }

    protected override void Execute()
    {
        var auditorium = Repository.GetById<Auditorium>(Id.GetValueOrDefault()) ?? new Auditorium();

        auditorium.DepartmentId = DepartmentId ?? 0;
        auditorium.BuildingId = BuildingId ?? 0;
        auditorium.Code = Code;
        auditorium.Kinds = Kinds;

        Repository.SaveOrUpdate(auditorium);
    }

    public class AsView : QueryBase<AddOrEditAuditoriumCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditAuditoriumCommand ExecuteResult()
        {
            var auditorium = Repository.GetById<Auditorium>(Id.GetValueOrDefault()) ?? new Auditorium();

            return new AddOrEditAuditoriumCommand
            {
                    Id = auditorium.Id,
                    Code = auditorium.Code,
                    DepartmentId = auditorium.DepartmentId,
                    BuildingId = auditorium.BuildingId,
                    Kinds = auditorium.Kinds.Select(r => new AuditoriumKind
                                      {
                                              Id = r.Id,
                                              Kind = r.Kind
                                      })
                                      .ToList()
            };
        }
    }
}
