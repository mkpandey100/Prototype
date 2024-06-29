using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prototype.Lord.Domain.AdminPortalModels.Tenants;

namespace Prototype.Lord.Infrastructure.Persistance.AdminPortalEntityConfiguration
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(t => t.Id);
        }
    }
}