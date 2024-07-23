using Prototype.Lord.Domain.Interfaces;

namespace Prototype.Lord.Application.Interfaces;

public interface IDatabaseMigrationService : IScopedService
{
    Task MigrateDatabaseAsync(string connectionString);
    Task DropDatabaseAsync(string connectionString);
}
