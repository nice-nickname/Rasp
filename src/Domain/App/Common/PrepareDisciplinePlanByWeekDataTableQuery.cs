using System.Data;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.App.Common;

public class PrepareDisciplinePlanByWeekDataTableQuery : QueryBase<DataTable>
{
    protected override DataTable ExecuteResult()
    {
        var bulkInsertDataTable = new DataTable(nameof(DisciplinePlanByWeek));
        bulkInsertDataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(DisciplinePlanByWeek.Id),
                DataType = typeof(int),
                AutoIncrement = true
        });
        bulkInsertDataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(DisciplinePlanByWeek.AssignmentHours),
                DataType = typeof(int),
        });
        bulkInsertDataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(DisciplinePlanByWeek.DisciplinePlanId),
                DataType = typeof(int),
        });
        bulkInsertDataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(DisciplinePlanByWeek.Week),
                DataType = typeof(int),
        });
        bulkInsertDataTable.PrimaryKey = new[] { bulkInsertDataTable.Columns[0] };

        return bulkInsertDataTable;
    }
}
