using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Domain.Interfaces;

namespace Prototype.Lord.Application.Interfaces;

public interface ICustomDbContextFactory : IScopedService
{
    DbContext CreateDbContext(string connectionString, ICurrentUserService currentUserService);
}
