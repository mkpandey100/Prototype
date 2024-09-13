using Prototype.Lord.Domain.Entities;
using Prototype.Lord.Domain.Enums;

namespace Prototype.Lord.Application.Interfaces;

public interface IUserCommandService
{
    Task<Status> CreateAsync(AppUser appUser, string password, string role);
}