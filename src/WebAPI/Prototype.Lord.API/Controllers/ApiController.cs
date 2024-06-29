using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Prototype.Lord.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    protected Guid CurrentUserId => Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == Constants.JwtId).Value);
    protected string Role => HttpContext.User.Claims.FirstOrDefault(x => x.Type == Constants.Role).Value;

    protected string FullName => HttpContext.User.Claims.FirstOrDefault(x => x.Type == Constants.FullName).Value;

    protected object HandleResult(HttpStatusCode statusCode, string message = null, List<string> errors = null)
    {
        return new
        {
            StatusCode = (int)statusCode,
            Message = message,
            Errors = errors
        };
    }

    /// <summary>
    /// Gets the action result value and returns it in desired format
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <param name="statusCode"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    protected object HandleResult<T>(HttpStatusCode statusCode, List<T> data = null, int totalRecord = 0, string message = null, List<string> errors = null)
    {
        return new
        {
            StatusCode = (int)statusCode,
            Message = message,
            Data = data ?? new List<T>(),
            Errors = errors,
            TaotalRecord = totalRecord
        };
    }

    protected object HandleResult<T>(HttpStatusCode statusCode, T data, string message = null, List<string> errors = null)
    {
        return new
        {
            StatusCode = (int)statusCode,
            Message = message,
            Data = data,
            Errors = errors
        };
    }
}

public static class Constants
{
    public const string UnauthorizedMessage = "You don't have permission to access this resource.";
    public const string SignUpSubject = "Welcome to Prototype", ProjectAction = "Prototype Project";
    public const string JwtId = "id", RoleAccess = "role_access", LoginId = "loginId", FirstLogin = "first_login", Role = "role", FullName = "fullName";
}

/// <summary>
/// Gets the action result value and returns it in desired format
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="message"></param>
/// <param name="statusCode"></param>
/// <returns></returns>