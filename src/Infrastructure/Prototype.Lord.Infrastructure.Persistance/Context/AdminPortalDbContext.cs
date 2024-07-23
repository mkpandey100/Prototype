using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain;
using Prototype.Lord.Domain.AdminPortalModels.Tenants;
using Prototype.Lord.Domain.AdminPortalModels.Users;
using System.Reflection;

namespace Prototype.Lord.Infrastructure.Persistance.Context;

public partial class AdminPortalDbContext : DbContext, IAdminPortalDbContext
{
    private readonly ICurrentUserService _currentUserService;
    public AdminPortalDbContext(DbContextOptions<AdminPortalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tenant> Tenants { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedById = _currentUserService.UserId;
                    entry.Entity.Created = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedById = _currentUserService.UserId;
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public Tenant GetTenant(string domain)
    {
        var tenant = Tenants.FirstOrDefault(t => t.SubDomain == domain && t.IsActive);
        tenant = new Tenant { Id = 1, Name = domain };
        return tenant ?? null;
    }

    public IList<Tenant> GetAllTenant()
    {
        return Tenants.Where(x => x.IsActive).ToList();
    }
}