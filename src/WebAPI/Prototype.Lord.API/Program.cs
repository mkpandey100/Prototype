using Asp.Versioning;
using Prototype.Lord.API.Extensions;
using Prototype.Lord.Application.Extensions;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Constants;
using Prototype.Lord.Infrastructure.Persistance.Extensions;
using Prototype.Lord.Infrastructure.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string[] origins = [];
#if DEBUG
origins = builder.Configuration["CORS_ORIGINS"]?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray();
#else
    origins =
 System.Environment.GetEnvironmentVariable("CORS_ORIGINS")?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray();
#endif

builder.Services.AddCors(options =>
{
    options.AddPolicy("PrototypeCorsPolicy",
        builder =>
        {
            builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .Build();
        });
});
string privateKey = builder.Configuration[Authentication.AuthKey] ??
                    System.Environment.GetEnvironmentVariable(Authentication.AuthKey);

builder.Services
    .AddSwaggerConfiguration()
    .AddPersistenceInfrastructure(builder.Configuration)
    .AddInfrastructureSharedLayer(builder.Configuration)
    .AddApplicationLayer()
    .AddWebApiLayer();

builder.Services.AddControllers();

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.Use(async (context, next) =>
//{
//    var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

//    // Get instance of TenantProviderService
//    var tenantProviderService = context.RequestServices.GetRequiredService<ITenantProviderService>();

//    // Get connection string based on referer
//    var connectionString = tenantProviderService.GetTenant();

//    // Set the retrieved connection string as the default connection string
//    var connectionStringSection = configuration.GetSection("ConnectionStrings");
//    connectionStringSection["DefaultConnection"] = connectionString.DbConnection;

//    await next.Invoke();
//});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("PrototypeCorsPolicy");

app.MapControllers();

app.Run();