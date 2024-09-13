using Microsoft.AspNetCore.Identity;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Entities;
using Prototype.Lord.Domain.Enums;
using Serilog;

namespace Prototype.Lord.Infrastructure.Identity.Extensions
{
    public class UserCommandService : IUserCommandService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserQueryService _userQueryService;

        public UserCommandService(UserManager<AppUser> userManager, IUserQueryService userQueryService)
        {
            _userManager = userManager;
            _userQueryService = userQueryService;
        }

        public async Task<Status> CreateAsync(AppUser appUser, string password, string role)
        {
            try
            {
                var result = await SaveUserAsync(appUser, password, role);
                return result.Succeeded ? Status.Success : throw new Exception(GenerateIdentityError(result, "User Create Failed."));
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private async Task<IdentityResult> SaveUserAsync(AppUser appUser, string password, string role)
        {
            appUser.UserName = appUser.Email;
            appUser.CreatedDate = DateTime.UtcNow;
            var userResult = await _userManager.CreateAsync(appUser, password);
            if (userResult.Succeeded)
            {
                var userRole = await _userManager.AddToRoleAsync(appUser, role);
                if (!userRole.Succeeded)
                    await _userManager.DeleteAsync(appUser);
            }

            return userResult;
        }

        private string GenerateIdentityError(IdentityResult result, string message)
        {
            string identityErrors = string.Empty;
            foreach (var item in result.Errors)
            {
                identityErrors = string.Concat(identityErrors, item.Code, ": ", item.Description);
            }
            var errors = result.Errors.Select(e => e.Description);
            return $"{message} {identityErrors}";
        }
    }
}
