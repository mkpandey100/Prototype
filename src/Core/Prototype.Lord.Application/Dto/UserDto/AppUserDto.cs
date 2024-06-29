using System.Security.Claims;

namespace Prototype.Lord.Application.Dto.UserDto;

public class AppUserDto : ClaimDto
{
    public ClaimsIdentity ClaimsIdentity { get; set; }
}