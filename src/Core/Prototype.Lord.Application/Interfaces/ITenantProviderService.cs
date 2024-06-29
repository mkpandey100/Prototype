using Prototype.Lord.Domain.AdminPortalModels.Tenants;

namespace Prototype.Lord.Application.Interfaces
{
    public interface ITenantProviderService : IDisposable
    {
        Tenant GetTenant();
    }
}