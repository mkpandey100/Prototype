using Microsoft.AspNetCore.Mvc;
using Prototype.Lord.API.Attributes;
using Prototype.Lord.Application.Common;
using Prototype.Lord.Application.Handlers.OrganizationalStandards.Commands;
using Prototype.Lord.Application.Handlers.OrganizationalStandards.Queries;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Constants;
using Prototype.Lord.Domain.Enums;
using System.Net;

namespace Prototype.Lord.API.Controllers.V1.OrganizationalStandards;

public class OrganizationalStandardController(IDapperRepository dapper) : ApiController
{
    private readonly IDapperRepository _dapper = dapper;

    [HttpPost]
    [Permission(Permissions.CreateOS)]
    public async Task<ActionResult<bool>> Create(CreateOrganizationalStandardCommand command)
    {
        var resp = await Mediator.Send(command);
        return resp.Status == Status.Success
           ? Ok(HandleResult(HttpStatusCode.OK, message: resp.Message))
           : BadRequest(HandleResult(HttpStatusCode.BadRequest, resp.Message, errors: resp.Errors));
    }

    [HttpPut]
    [Permission(Permissions.UpdateOS)]
    public async Task<ActionResult<bool>> Update(UpdateOrganizationalStandardCommand command)
    {
        command.Permissions = CurrentUserId.GetUserPermissions(_dapper);
        var resp = await Mediator.Send(command);
        return resp.Status == Status.Success
           ? Ok(HandleResult(HttpStatusCode.OK, message: resp.Message))
           : BadRequest(HandleResult(HttpStatusCode.BadRequest, resp.Message, errors: resp.Errors));
    }

    [HttpDelete("{id}")]
    [Permission(Permissions.DeleteOS)]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var resp = await Mediator.Send(new DeleteOrganizationalStandardCommand { Id = id, Permissions = CurrentUserId.GetUserPermissions(_dapper) });
        return resp.Status == Status.Success
           ? Ok(HandleResult(HttpStatusCode.OK, message: resp.Message))
           : BadRequest(HandleResult(HttpStatusCode.BadRequest, resp.Message, errors: resp.Errors));
    }

    [HttpGet]
    [Permission(Permissions.ReadOS)]
    [ProducesResponseType(typeof(ListResponseOutputDto<OrganizationalStandardResponseDto>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOrganizationalStandardQuery organizationalStandardInput)
    {
        var resp = await Mediator.Send(organizationalStandardInput);
        return Ok(HandleResult(HttpStatusCode.OK, data: resp.Data, resp.Message, errors: resp.Errors));
    }

    [HttpGet("{id}")]
    [Permission(Permissions.ReadOS)]
    [ProducesResponseType(typeof(ResponseOutputDto<OrganizationalStandardDetailResponseDto>), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        var resp = await Mediator.Send(new GetOrganizationalStandardQuery { Id = id });
        return Ok(HandleResult(HttpStatusCode.OK, data: resp.Data, resp.Message, errors: resp.Errors));
    }
}