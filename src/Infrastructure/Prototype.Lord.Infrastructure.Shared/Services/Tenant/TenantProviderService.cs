using Microsoft.AspNetCore.Http;
using Prototype.Lord.Application.Interfaces;

namespace Prototype.Lord.Infrastructure.Shared.Services.Tenant
{
    public class TenantProviderService : ITenantProviderService
    {
        private readonly Domain.AdminPortalModels.Tenants.Tenant Tenant;
        private readonly IAdminPortalDbContext _tenantContext;

        public TenantProviderService(IHttpContextAccessor accessor, IAdminPortalDbContext context)
        {
            _tenantContext = context;

            var host = GetHostFromRequest(accessor);
            if (!string.IsNullOrEmpty(host))
            {
                Tenant = _tenantContext.GetTenant(host);
                if (Tenant == null)
                {
                    throw new Exception("Tenant not found.");
                }
            }
            else
            {
                throw new Exception("Host not provided.");
            }
        }

        private string GetHostFromRequest(IHttpContextAccessor accessor)
        {
            var host = string.Empty;
            if (accessor.HttpContext != null)
            {
                if (Guid.TryParse(accessor.HttpContext.Request.Headers["Tenant"], out Guid tenantId))
                {
                    host = _tenantContext.Tenants.Find(tenantId)?.SubDomain;
                }
                else
                {
                    host = accessor.HttpContext.Request.Headers["Referer"].ToString();
                    host = host.Split('/')[2];
                }
            }

            return host;
        }

        public Domain.AdminPortalModels.Tenants.Tenant GetTenant()
        {
            return Tenant;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _tenantContext.Dispose();
                }
            }
            _disposed = true;
        }
    }
}