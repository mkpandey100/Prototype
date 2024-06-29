using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prototype.Lord.Domain.Extensions;
using Prototype.Lord.Domain.Interfaces;

namespace Prototype.Lord.Infrastructure.Shared.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureSharedLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddServicesForInterface(typeof(ITransientService), ServiceLifetime.Transient)
        .AddServicesForInterface(typeof(IScopedService), ServiceLifetime.Scoped)
        .AddServicesForInterface(typeof(ISingletonService), ServiceLifetime.Singleton);

        return services;
    }
}