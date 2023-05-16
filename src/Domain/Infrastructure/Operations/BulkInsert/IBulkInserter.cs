using System.Data;

namespace Domain.Infrastructure;

public interface IBulkInserter : IDisposable
{
    void Write(DataTable table);
}
