using Domain.Infrastructure.Providers;
using Domain.Persistence;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;

namespace Domain.App.Common;

public class BulkInsertDisciplinePlanByWeekCommand : CommandBase
{
    public List<DisciplinePlanByWeek>? Rows { get; set; }

    protected override void Execute()
    {
        if (Rows == null || !Rows.Any())
        {
            return;
        }

        var dataTable = Dispatcher.Query(new PrepareDisciplinePlanByWeekDataTableQuery());

        foreach (var dataRow in Rows.Select(s =>
                 {
                     var row = dataTable.NewRow();
                     row[nameof(DisciplinePlanByWeek.DisciplinePlanId)] = s.DisciplinePlanId;
                     row[nameof(DisciplinePlanByWeek.AssignmentHours)] = s.AssignmentHours;
                     row[nameof(DisciplinePlanByWeek.Week)] = s.Week;
                     return row;
                 }))
        {
            dataTable.Rows.Add(dataRow);
        }

        var val = IoCFactory.Instance.TryResolve<IBulkInserterProvider>().Provide();
        val.Write(dataTable);
    }
}
