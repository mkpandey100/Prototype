namespace Prototype.Lord.Domain.Constants;

public static class Authentication
{
    public const string UnauthorizedMessage = "You don't have permission to access this resource.";
    public const string JwtId = "id", LoginId = "loginId", AuthKey = "JwtPrivateKey", IsAdmin = "isAdmin";
    public const string UserName = "username", FullName = "fullname";
}