using MediatR;
using Prototype.Lord.Application.Common;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Enums;

namespace Prototype.Lord.Application.Handlers.Tenants.Commands.DeleteTenant;

public class DeleteTenantCommand : IRequest<OutputDto>
{
    public int Id { get; set; }
}

public class DeleteTenantCommandHandler(IDatabaseMigrationService databaseMigrationService, IAdminPortalDbContext adminPortalDbContext) : IRequestHandler<DeleteTenantCommand, OutputDto>
{
    private readonly IDatabaseMigrationService _databaseMigrationService = databaseMigrationService;
    private readonly IAdminPortalDbContext _adminPortalDbContext = adminPortalDbContext;

    public async Task<OutputDto> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tenant = _adminPortalDbContext.Tenants.FirstOrDefault(x => x.Id == request.Id);
            _adminPortalDbContext.Tenants.Remove(tenant);

            //drop database
            await _databaseMigrationService.DropDatabaseAsync(tenant.DbConnection);

            await _adminPortalDbContext.SaveChangesAsync(cancellationToken);

            return new OutputDto
            {
                Status = Status.Success,
                Message = $"Successfully deleted Tenant {tenant.Name}."
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}