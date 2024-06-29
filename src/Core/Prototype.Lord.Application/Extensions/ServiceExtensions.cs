using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Prototype.Lord.Application.Behaviors;
using Prototype.Lord.Domain.Extensions;
using Prototype.Lord.Domain.Interfaces;
using System.Reflection;

namespace Prototype.Lord.Application.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services
        .AddServicesForInterface(typeof(ITransientService), ServiceLifetime.Transient)
        .AddServicesForInterface(typeof(IScopedService), ServiceLifetime.Scoped);
        return services;
    }
}