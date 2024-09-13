using Prototype.Lord.Domain.Entities;

namespace Prototype.Lord.Application.Interfaces;

public interface IUserQueryService
{
    Task<bool> CheckPasswordAsync(AppUser user, string password);

    Task<AppUser> FindByIdAsync(Guid userId);

    Task<AppUser> FindByEmailAsync(string email);

    Task<bool> CheckUserAdminAsync(Guid userId);
}