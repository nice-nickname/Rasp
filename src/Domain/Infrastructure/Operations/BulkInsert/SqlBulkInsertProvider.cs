using System.Data.SqlClient;
using System.Security;
using Domain.Infrastructure.Providers;

namespace Domain.Infrastructure;

public class SqlBulkInsertProvider : IBulkInserterProvider
{
    private readonly string _connectionString;

    private readonly SecureString _password;

    private readonly string _user;

    public SqlBulkInsertProvider(string connectionString)
    {
        var cb = new SqlConnectionStringBuilder(connectionString);
        this._user = cb.UserID;
        this._password = new SecureString();
        foreach (var c in cb.Password)
        {
            this._password.AppendChar(c);
        }

        this._password.MakeReadOnly();
        cb.Remove("User");
        cb.Remove("UserID");
        cb.Remove("User ID");
        cb.Remove("Password");
        this._connectionString = cb.ConnectionString;
    }

    public IBulkInserter Provide()
    {
        return new SqlBulkInserter(new SqlConnection
        {
                ConnectionString = this._connectionString,
                Credential = new SqlCredential(this._user, this._password)
        });
    }
}
