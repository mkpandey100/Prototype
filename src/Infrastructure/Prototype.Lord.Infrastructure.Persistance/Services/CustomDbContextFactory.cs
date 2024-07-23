using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Interfaces;
using Prototype.Lord.Infrastructure.Persistance.Context;

namespace Prototype.Lord.Infrastructure.Persistance.Services;

public class CustomDbContextFactory : ICustomDbContextFactory
{
    private readonly IDateTime _dateTime;

    public CustomDbContextFactory(IDateTime dateTime)
    {
        _dateTime = dateTime;
    }

    public DbContext CreateDbContext(string connectionString, ICurrentUserService currentUserService)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options, currentUserService, _dateTime);
    }
}
