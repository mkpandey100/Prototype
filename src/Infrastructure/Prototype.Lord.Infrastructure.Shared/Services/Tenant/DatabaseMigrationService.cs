using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Interfaces;
using Prototype.Lord.Infrastructure.Persistance.Context;

namespace Prototype.Lord.Infrastructure.Shared.Services.Tenant;

public class DatabaseMigrationService(IDateTime dateTime, ICurrentUserService currentUserService) : IDatabaseMigrationService
{
    public async Task MigrateDatabaseAsync(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        using var context = new ApplicationDbContext(optionsBuilder.Options, currentUserService, dateTime);
        context.Database.SetCommandTimeout(600);

        var migrationCount = await context.Database.GetAppliedMigrationsAsync();
        if (!migrationCount.Any())
        {
            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Migration failed: {ex.Message}");
            }
        }
    }

    public async Task DropDatabaseAsync(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        using var context = new ApplicationDbContext(optionsBuilder.Options, currentUserService, dateTime);
        await context.Database.EnsureDeletedAsync();
    }
}
