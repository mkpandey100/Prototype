using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Prototype.Lord.Application.Dto.UserDto;
using Prototype.Lord.Domain.Interfaces;
using System.Text;
using Prototype.Lord.Domain.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Prototype.Lord.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Prototype.Lord.Infrastructure.Persistance.Context;

namespace Prototype.Lord.Infrastructure.Identity.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
        {
            services
               .AddServicesForInterface(typeof(ITransientService), ServiceLifetime.Transient)
               .AddServicesForInterface(typeof(IScopedService), ServiceLifetime.Scoped)
               .AddServicesForInterface(typeof(ISingletonService), ServiceLifetime.Singleton);

            return services;
        }

        public static IServiceCollection AddIdentityAuthInfrastructure(this IServiceCollection services, string privateKey, JwtIssuerOptions config)
        {
            SymmetricSecurityKey _signingKey = new(Encoding.ASCII.GetBytes(privateKey));
            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = config.Issuer;
                options.Audience = config.Audience;
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config.Issuer,
                ValidateAudience = true,
                ValidAudience = config.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.ClaimsIssuer = config.Issuer;
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (path.StartsWithSegments("/notifications"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var te = context.Exception;
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(1);
            });
            services.AddAuthorization();
            services.AddSignalR();
            return services;
        }
    }
}
