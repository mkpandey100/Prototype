using Prototype.Lord.Application.Dto.UserDto;
using Prototype.Lord.Domain.Interfaces;
using System.Security.Claims;

namespace Prototype.Lord.Application.Interfaces
{
    public interface IJwtService : IScopedService
    {
        ClaimsIdentity GenerateClaimsIdentity(ClaimDto claimDTO);

        Task<AuthenticationResponseDto> GenerateJwt(AppUserDto userDetails);
    }
}
