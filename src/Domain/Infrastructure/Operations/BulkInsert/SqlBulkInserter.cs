using System.Data;
using System.Data.SqlClient;

namespace Domain.Infrastructure;

public class SqlBulkInserter : IBulkInserter
{
    private readonly SqlConnection _connection;

    public SqlBulkInserter(SqlConnection connection)
    {
        this._connection = connection;
        this._connection.Open();
    }

    public void Write(DataTable table)
    {
        using var copy = new SqlBulkCopy(this._connection);

        copy.DestinationTableName = table.TableName;
        for (var i = 0; i < table.Columns.Count; i++)
        {
            var col = table.Columns[i];
            if (col.ColumnName == "Id")
                continue;

            copy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
        }

        copy.WriteToServer(table);
    }

    public void Dispose()
    {
        this._connection.Dispose();
    }
}
