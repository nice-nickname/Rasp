using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.Api;

public class GetWeekendsForWeekQuery : QueryBase<List<DateOnly>>
{
    public DateTime StartDate { get; set; }

    protected override List<DateOnly> ExecuteResult()
    {
        var startDate = DateOnly.FromDateTime(StartDate);
        var endDate = DateOnly.FromDateTime(StartDate.AddDays(7));

        return Repository.Query<Holidays>()
                         .Where(r => r.Date >= startDate
                                  && r.Date <= endDate)
                         .Select(r => r.Date)
                         .ToList();
    }
}
