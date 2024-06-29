namespace Prototype.Lord.Application.Dto.UserDto;

public class AuthenticationRequestDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string BrowserName { get; set; }
    public string OsName { get; set; }
}