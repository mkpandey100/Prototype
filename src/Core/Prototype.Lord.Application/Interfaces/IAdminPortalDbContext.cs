using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Domain.AdminPortalModels.Tenants;
using Prototype.Lord.Domain.AdminPortalModels.Users;

namespace Prototype.Lord.Application.Interfaces;

public interface IAdminPortalDbContext
{
    DbSet<Tenant> Tenants { get; set; }
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    Tenant GetTenant(string domain);

    IList<Tenant> GetAllTenant();

    void Dispose();
}