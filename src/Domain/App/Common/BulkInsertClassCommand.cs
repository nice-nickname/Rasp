using Domain.Infrastructure.Providers;
using Domain.Persistence;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;

namespace Domain.App.Common;

public class BulkInsertClassCommand : CommandBase
{
    public List<Class>? Classes { get; set; }

    protected override void Execute()
    {
        if (Classes == null || !Classes.Any())
            return;

        var dataTable = Dispatcher.Query(new PrepareClassDataTableQuery());

        foreach (var @class in Classes)
        {
            var row = dataTable.NewRow();
            row[nameof(Class.Day)] = @class.Day;
            row[nameof(Class.Week)] = @class.Week;
            row[nameof(Class.SubGroupNo)] = @class.SubGroupNo;
            row[nameof(Class.IsUnwanted)] = @class.IsUnwanted;
            row[nameof(Class.AuditoriumId)] = @class.AuditoriumId;
            row[nameof(Class.ScheduleFormatId)] = @class.ScheduleFormatId;
            row[nameof(Class.DisciplinePlanId)] = @class.DisciplinePlanId;
            dataTable.Rows.Add(row);
        }

        var val = IoCFactory.Instance.TryResolve<IBulkInserterProvider>().Provide();
        val.Write(dataTable);
    }
}
