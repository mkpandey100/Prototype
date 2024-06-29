using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Infrastructure.Shared.Services.Tenant;

namespace Prototype.Lord.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddWebApiLayer(this IServiceCollection services)
    {
        services.AddScoped<ITenantProviderService, TenantProviderService>();

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        return services;
    }

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "OS API", Version = "v1" });

            // Add security definition for JWT token
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            // Add security requirement for JWT token
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Define custom headers and add security definitions and requirements
            string[] customHeaders = ["Referer"];
            foreach (var header in customHeaders)
            {
                option.AddSecurityDefinition(header, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = $"{header} of the project",
                    Name = header,
                    Type = SecuritySchemeType.ApiKey
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = header
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            }
        });

        return services;
    }
}