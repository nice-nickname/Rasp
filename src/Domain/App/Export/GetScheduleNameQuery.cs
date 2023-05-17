using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Export;

public class GetScheduleNameQuery : QueryBase<string>
{
    public int? GroupId { get; set; }

    public int? AuditoriumId { get; set; }

    public int? TeacherId { get; set; }

    protected override string ExecuteResult()
    {
        var name = string.Empty;

        if (GroupId.HasValue)
        {
            name = Repository.GetById<Group>(GroupId).Code;
        }
        else if (AuditoriumId.HasValue)
        {
            var auditorium = Repository.GetById<Auditorium>(AuditoriumId);
            name = $"{auditorium.Building.Name}-{auditorium.Code}";
        }
        else if (TeacherId.HasValue)
        {
            name = Repository.GetById<Teacher>(TeacherId).Name;
        }

        return name;
    }
}
