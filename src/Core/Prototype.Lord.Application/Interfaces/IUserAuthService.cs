using Prototype.Lord.Application.Dto.UserDto;
using Prototype.Lord.Domain.Interfaces;

namespace Prototype.Lord.Application.Interfaces;

public interface IUserAuthService : IScopedService
{
    Task<AuthenticationResponseDto> AuthenticateAsync(AuthenticationRequestDto request);
}
