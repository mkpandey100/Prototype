namespace Prototype.Lord.Domain.Constants;

public static class ProcedureConstants
{
    public const string GetUserRolePermissionDetails = "usp_GetUserRolePermissionDetails";
    public const string GetAllRolePermissionDetails = "usp_GetAllRolePermissionDetails";
    public const string GetRolePermissionDetailsByRoleId = "usp_GetRolePermissionDetailsByRoleId";
    public const string GetUserRolePermissionDetailsForPermissionCheck = "[usp_GetUserRolePermissionDetailsForPermissionCheck]";

    // project
    public const string ReadAllProject = "[dbo].[usp_GetAllProjects]";

    // refresh token
    public const string GetUserDetailByIdForClaim = "[usp_GetUserDetailByIdForClaim]";
    public const string SaveRefreshToken = "[usp_SaveRefreshToken]";
    public const string GetUserDetailByRefreshTokenForClaim = "[usp_GetUserDetailByRefreshTokenForClaim]";
    public const string RevokeRefreshToken = "[]";
}