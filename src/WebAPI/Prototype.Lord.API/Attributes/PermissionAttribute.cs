using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Prototype.Lord.API.Controllers;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Constants;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace Prototype.Lord.API.Attributes;

public class PermissionAttribute : TypeFilterAttribute
{
    private string _actionName = string.Empty;

    public PermissionAttribute(params string[] claimValues) : base(typeof(ClaimRequirementFilter))
    {
        var claims = GetClaims(claimValues).ToArray();
        Arguments = new object[] { claims };
    }

    private IEnumerable<Claim> GetClaims(string[] claimValues)
    {
        foreach (var item in claimValues)
        {
            yield return new Claim(_actionName, item);
        }
    }

    public class ClaimRequirementFilter : IAsyncActionFilter
    {
        private readonly Claim[] _claims;
        private readonly IDapperRepository _dapper;
        private readonly IApplicationDbContext _dbContext;

        public ClaimRequirementFilter(Claim[] claims, IDapperRepository dapper, IApplicationDbContext dbContext)
        {
            _claims = claims;
            _dapper = dapper;
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isWeb = Convert.ToBoolean(context.HttpContext.Request.Headers["IsWeb"]);
            var user = context.HttpContext.User;
            Guid userId = Guid.TryParse(user.FindFirst(Constants.JwtId).Value, out Guid usId) ? usId : Guid.Empty;

            var loggedInUser = await _dbContext.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            if (loggedInUser is null) ReturnStatus(context, (int)HttpStatusCode.Unauthorized);

            (IReadOnlyCollection<string> userPermissions, bool isRefreshTokenValid) = await GetUserPermissionsAsync(userId);
            if (isWeb && isRefreshTokenValid)
            {
                ReturnStatus(context, (int)HttpStatusCode.Locked);
            }
            else
            {
                if (userPermissions.Count > 0)
                {
                    try
                    {
                        var claimsValues = _claims.Select(w => w.Value).ToList();
                        if (userPermissions.Any(q => claimsValues.Contains(q)))
                        {
                            await next();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }

                ReturnStatus(context, (int)HttpStatusCode.Forbidden);
                return;
            }
        }

        private static void ReturnStatus(ActionExecutingContext context, int code)
        {
            context.HttpContext.Response.StatusCode = code;
        }

        private async Task<(IReadOnlyCollection<string>, bool)> GetUserPermissionsAsync(Guid userId)
        {
            string outputParameter = "refreshTokenStatus";
            var param = new DynamicParameters();
            param.Add("@userId", userId);
            param.Add($"@{outputParameter}", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            (object output, List<string> result) = await _dapper.QueryAllAsync<string>(ProcedureConstants.GetUserRolePermissionDetailsForPermissionCheck, param, outputParameter);
            return (result.GroupBy(q => q).Select(q => q.Key).ToList(), (bool)output);
        }
    }
}

public static class CheckPermissionExtension
{
    private static IDapperRepository _dapper;

    public static IReadOnlyCollection<string> GetUserPermissions(this Guid userId, IDapperRepository dapper)
    {
        _dapper = dapper;
        return GetUserPermissions(userId);
    }

    private static IReadOnlyCollection<string> GetUserPermissions(Guid userId)
    {
        string outputParameter = "refreshTokenStatus";
        var param = new DynamicParameters();
        param.Add("@userId", userId);
        param.Add($"@{outputParameter}", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        (object output, List<string> result) = _dapper.QueryAllAsync<string>(ProcedureConstants.GetUserRolePermissionDetailsForPermissionCheck, param, outputParameter).Result;
        return result.GroupBy(q => q).Select(q => q.Key).ToList();
    }
}