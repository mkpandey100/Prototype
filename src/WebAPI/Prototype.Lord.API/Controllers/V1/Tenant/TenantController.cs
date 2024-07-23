using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prototype.Lord.Application.Handlers.Tenants.Commands.CreateTenant;
using Prototype.Lord.Application.Handlers.Tenants.Commands.DeleteTenant;
using Prototype.Lord.Domain.Enums;
using System.Net;

namespace Prototype.Lord.API.Controllers.V1
{
    public class TenantController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Create(CreateTenantCommand command)
        {
            var resp = await Mediator.Send(command);
            return resp.Status == Status.Success
               ? Ok(HandleResult(HttpStatusCode.OK, message: resp.Message))
               : BadRequest(HandleResult(HttpStatusCode.BadRequest, resp.Message, errors: resp.Errors));
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var resp = await Mediator.Send(new DeleteTenantCommand { Id = id});
            return resp.Status == Status.Success
               ? Ok(HandleResult(HttpStatusCode.OK, message: resp.Message))
               : BadRequest(HandleResult(HttpStatusCode.BadRequest, resp.Message, errors: resp.Errors));
        }
    }
}
