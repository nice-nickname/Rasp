using System.Data;
using Domain.Persistence;
using Incoding.Core.CQRS.Core;

namespace Domain.App.Common;

public class PrepareClassDataTableQuery : QueryBase<DataTable>
{
    protected override DataTable ExecuteResult()
    {
        var dataTable = new DataTable(nameof(Class));
        dataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(Class.Id),
                DataType = typeof(int),
                AutoIncrement = true
        });
        dataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(Class.Day),
                DataType = typeof(DayOfWeek),
        });
        dataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(Class.Week),
                DataType = typeof(int),
        });
        dataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(Class.SubGroupNo),
                DataType = typeof(int),
        });
        dataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(Class.AuditoriumId),
                DataType = typeof(int),
                AllowDBNull = true
                
        });
        dataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(Class.ScheduleFormatId),
                DataType = typeof(int),
        });
        dataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(Class.DisciplinePlanId),
                DataType = typeof(int),
        });
        dataTable.Columns.Add(new DataColumn
        {
                ColumnName = nameof(Class.IsUnwanted),
                DataType = typeof(bool),
        });

        return dataTable;
    }
}
