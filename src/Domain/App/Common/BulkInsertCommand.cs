using System.Data;
using System.Data.SqlClient;
using System.Security;
using Domain.Infrastructure.Providers;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;

namespace Domain.App.Common;

public class BulkInsertCommand : CommandBase
{
    public DataTable? Table { get; set; }

    public List<DataRow>? Rows { get; set; }

    protected override void Execute()
    {
        if (Table == null || Table.Columns.Count == 0 || Rows == null || Rows.Count == 0)
        {
            return;
        }

        foreach (var dataRow in Rows)
        {
            Table.Rows.Add(dataRow);
        }
        var val = IoCFactory.Instance.TryResolve<IBulkInserterProvider>().Provide();
        val.Write(Table);
    }
}
