namespace Domain.Infrastructure.Providers;

public interface IBulkInserterProvider
{
    IBulkInserter Provide();
}
