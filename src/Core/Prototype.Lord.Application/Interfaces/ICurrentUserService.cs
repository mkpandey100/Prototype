using Prototype.Lord.Domain.Interfaces;

namespace Prototype.Lord.Application.Interfaces;

public interface ICurrentUserService : IScopedService
{
    Guid UserId { get; }
    string UserName { get; }
    string FullName { get; }
    string Role { get; }
}