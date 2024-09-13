using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Domain.Entities;

namespace Prototype.Lord.Application.Interfaces;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    public DbSet<AppUser> Users { get; set; }
    public DbSet<AppRole> Roles { get; set; }
    public DbSet<IdentityUserRole<Guid>> UserRoles { get; set; }
}