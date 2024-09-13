using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prototype.Lord.API.Controllers;
using Prototype.Lord.Application.Dto.UserDto;
using Prototype.Lord.Application.Interfaces;
using System.Net;

namespace eLearning.Api.Controllers.V1.Auth;

[ApiVersion("1")]
public class AuthenticationController(IUserAuthService authService) : ApiController
{
    private readonly IUserAuthService _authService = authService;

    [AllowAnonymous]
    [HttpPost("token")]
    public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequestDto request)
    {
        try
        {
            var result = await _authService.AuthenticateAsync(request);
            return Ok(HandleResult(HttpStatusCode.OK, data: result));
        }
        catch (Exception ex)
        {
            return BadRequest(HandleResult(HttpStatusCode.BadRequest, ex.Message, null));
        }
    }
}
