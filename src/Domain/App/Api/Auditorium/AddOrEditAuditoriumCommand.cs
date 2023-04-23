using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class AddOrEditAuditoriumCommand : CommandBase
{
    public int? Id { get; set; }

    public int? DepartmentId { get; set; }

    public int? BuildingId { get; set; }

    public int Capacity { get; set; }

    public string Code { get; set; }

    public List<TempAuditoriumKind> Kinds { get; set; }

    protected override void Execute()
    {
        var auditorium = Repository.GetById<Auditorium>(Id.GetValueOrDefault()) ?? new Auditorium();

        auditorium.DepartmentId = DepartmentId ?? 0;
        auditorium.BuildingId = BuildingId ?? 0;
        auditorium.Code = Code;
        auditorium.Capacity = Capacity;
        auditorium.Kinds = Kinds.Where(r => r.IsSelected)
                                .Select(r => new AuditoriumKind
                                {
                                        Id = r.Id,
                                        Kind = r.Kind
                                })
                                .ToList();

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
                    Kinds = auditorium.Kinds.Select(r => new TempAuditoriumKind
                                      {
                                              Id = r.Id,
                                              Kind = r.Kind,
                                              IsSelected = true
                                      })
                                      .ToList(),
                    Capacity = auditorium.Capacity
            };
        }
    }

    public class TempAuditoriumKind : AuditoriumKind
    {
        public bool IsSelected { get; set; }
    }
}
