using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Prototype.Lord.Application.Dto.UserDto;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Extensions;
using Prototype.Lord.Domain.Interfaces;
using Prototype.Lord.Infrastructure.Persistance.Context;
using Prototype.Lord.Infrastructure.Persistance.Repository;
using System.Text;

namespace Prototype.Lord.Infrastructure.Persistance.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AdminPortalDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AdminPortalConnection")));
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAdminPortalDbContext, AdminPortalDbContext>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddScoped<IDapperRepository>(q => new DapperRepository(configuration.GetConnectionString("DefaultConnection")));

        string secretKey = configuration.GetSection("JwtPrivateKey").Value;
        SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

        var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

        // Configure JwtIssuerOptions
        services.Configure<JwtIssuerOptions>(options =>
        {
            options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
            options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            options.JwtPrivateKey = secretKey;
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

            ValidateAudience = true,
            ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

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
            options.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            options.TokenValidationParameters = tokenValidationParameters;
            options.SaveToken = true;
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/notify")))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromDays(1);
        });
        services.AddAuthorization();

        services
        .AddServicesForInterface(typeof(ITransientService), ServiceLifetime.Transient)
        .AddServicesForInterface(typeof(IScopedService), ServiceLifetime.Scoped);
        return services;
    }
}