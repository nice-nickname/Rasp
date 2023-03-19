using Incoding.Core.CQRS.Core;

namespace Domain.API;

public class AddOrEditScheduleTable : CommandBase
{
    public int? Id { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    protected override void Execute()
    {
        var record = Repository.GetById<ScheduleTable>(Id) ?? new ScheduleTable();

        record.StartTime = Start;
        record.EndTime = End;
        Repository.SaveOrUpdate(record);
    }
}