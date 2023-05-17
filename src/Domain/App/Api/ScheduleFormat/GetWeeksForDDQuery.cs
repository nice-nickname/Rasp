using Domain.Persistence;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Domain.Api;

public class GetWeeksForDDQuery : QueryBase<List<KeyValueVm>>
{
    public int FacultyId { get; set; }

    protected override List<KeyValueVm> ExecuteResult()
    {
        var weeks = Dispatcher.Query(new GetFacultySettingQuery<int>
        {
                FacultyId = FacultyId,
                Type = FacultySettings.OfType.CountOfWeeks
        });

        return Enumerable.Range(1, weeks)
                         .Select(s => new KeyValueVm(s, s.ToString()))
                         .ToList();
    }
}
