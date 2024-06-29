using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain;
using Prototype.Lord.Domain.BizVueModels;
using Prototype.Lord.Domain.BizVueModels.Projects;
using Prototype.Lord.Domain.BizVueModels.Tasks;
using Prototype.Lord.Domain.Interfaces;
using System.Reflection;

namespace Prototype.Lord.Infrastructure.Persistance.Context;

public partial class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly Domain.AdminPortalModels.Tenants.Tenant _tenant;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService, IDateTime dateTime) : base(options)
    {
        _currentUserService = currentUserService;
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

    public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Tasks> Tasks { get; set; }
    public virtual DbSet<OrganizationalStandard> OrganizationalStandards { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var configurationAssembly = Assembly.GetExecutingAssembly();
        var configurationTypes = configurationAssembly.GetTypes()
            .Where(t => t.Namespace == "Prototype.Lord.Infrastructure.Persistance.Configurations.ApplicationDb"
                        && typeof(IEntityTypeConfiguration<>).IsAssignableFrom(t));

        foreach (var configurationType in configurationTypes)
        {
            dynamic configurationInstance = Activator.CreateInstance(configurationType);
            builder.ApplyConfiguration(configurationInstance);
        }

        base.OnModelCreating(builder);
    }
}