using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Domain.BizVueModels;
using Prototype.Lord.Domain.BizVueModels.Projects;
using Prototype.Lord.Domain.BizVueModels.Tasks;

namespace Prototype.Lord.Application.Interfaces;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    DbSet<AspNetUsers> AspNetUsers { get; set; }
    DbSet<Project> Projects { get; set; }
    DbSet<Tasks> Tasks { get; set; }
    DbSet<OrganizationalStandard> OrganizationalStandards { get; set; }
}