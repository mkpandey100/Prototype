namespace Prototype.Lord.Domain.Constants;

public static class BasicConstants
{
    public const string CorsOrigins = "CORS_ORIGINS";
    public const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";
    public const string DbConnection = "DefaultConnection";
    public const int DefaultPageOffset = 10;
}

public static class JwtClaimIdentifiers
{
    public const string Rol = "rol", Id = "id";
}

public static class JwtClaims
{
    public const string ApiAccess = "api_access";
}