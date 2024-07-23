using MediatR;
using Prototype.Lord.Application.Common;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Enums;

namespace Prototype.Lord.Application.Handlers.Tenants.Commands.CreateTenant;

public class CreateTenantCommand : IRequest<OutputDto>
{
    public string Name { get; set; }
}

public class CreateTenantCommandHandler(IDatabaseMigrationService databaseMigrationService, IAdminPortalDbContext adminPortalDbContext) : IRequestHandler<CreateTenantCommand, OutputDto>
{
    private readonly IDatabaseMigrationService _databaseMigrationService = databaseMigrationService;
    private readonly IAdminPortalDbContext _adminPortalDbContext = adminPortalDbContext;

    public async Task<OutputDto> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tenant = new Prototype.Lord.Domain.AdminPortalModels.Tenants.Tenant
            {
                Name = request.Name,
                DbConnection = $"Server=.;Database=Prototype.{request.Name};Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;"
            };

            _adminPortalDbContext.Tenants.Add(tenant);
            await _adminPortalDbContext.SaveChangesAsync(cancellationToken);

            await _databaseMigrationService.MigrateDatabaseAsync(tenant.DbConnection);
            return new OutputDto
            {
                Status = Status.Success,
                Message = $"Successfully created Tenant {request.Name}."
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}