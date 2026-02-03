using System.Threading;
using System.Threading.Tasks;
using Mbrcld.Application.Features.Accounts.Commands;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mbrcld.Web.API
{
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/account")]
    public sealed class AccountController : BaseController
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            command.UserId = User.GetUserId();
            var result = await this.mediator.Send(command, cancellationToken);
            return FromResult(result);
        }

        [HttpPost("change-email")]
        public async Task<ActionResult> ChangeEmail(ChangeEmailCommand command, CancellationToken cancellationToken)
        {
            command.UserId = User.GetUserId();
            var result = await this.mediator.Send(command, cancellationToken);
            return FromResult(result);
        }

        [HttpPost("delete-account")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        public async Task<ActionResult> DeleteAccount(DeleteAccountCommand command, CancellationToken cancellationToken)
        {
            var result = await this.mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
