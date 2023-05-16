using Domain.App.Api;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;
using Resources;

namespace Domain.Api;

public class GetDaysOfWeekForDDQuery : QueryBase<List<KeyValueVm>>
{
    public int FacultyId { get; set; }

    public int Week { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        var startDate = Dispatcher.Query(new GetDateFromWeekQuery
        {
                FacultyId = FacultyId,
                Week = Week
        });

        var weekends = Dispatcher.Query(new GetWeekendsForWeekQuery
        {
                FacultyId = FacultyId,
                StartDate = startDate
        });

        return new List<KeyValueVm>
        {
                new()
                {
                        Text = DataResources.Monday,
                        Value = ((int)DayOfWeek.Monday).ToString(),
                        CssClass = weekends.Contains(DateOnly.FromDateTime(startDate)) ? "disabled" : null
                },
                new()
                {
                        Text = DataResources.Tuesday,
                        Value = ((int)DayOfWeek.Tuesday).ToString(),
                        CssClass = weekends.Contains(DateOnly.FromDateTime(startDate.AddDays(1))) ? "disabled" : null
                },
                new()
                {
                        Text = DataResources.Wednesday,
                        Value = ((int)DayOfWeek.Wednesday).ToString(),
                        CssClass = weekends.Contains(DateOnly.FromDateTime(startDate.AddDays(2))) ? "disabled" : null
                },
                new()
                {
                        Text = DataResources.Thursday,
                        Value = ((int)DayOfWeek.Thursday).ToString(),
                        CssClass = weekends.Contains(DateOnly.FromDateTime(startDate.AddDays(3))) ? "disabled" : null
                },
                new()
                {
                        Text = DataResources.Friday,
                        Value = ((int)DayOfWeek.Friday).ToString(),
                        CssClass = weekends.Contains(DateOnly.FromDateTime(startDate.AddDays(4))) ? "disabled" : null
                },
                new()
                {
                        Text = DataResources.Saturday,
                        Value = ((int)DayOfWeek.Saturday).ToString(),
                        CssClass = weekends.Contains(DateOnly.FromDateTime(startDate.AddDays(5))) ? "disabled" : null
                },
        };
    }
}
