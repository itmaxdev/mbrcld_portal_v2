using Mbrcld.Application.Features.Accounts.Commands;
using Mbrcld.Web.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/reset-password")]
    public sealed class ResetPasswordController : BaseController
    {
        private readonly IMediator mediator;

        public ResetPasswordController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var result = await this.mediator.Send(command, cancellationToken);
            return FromResult(result);
        }
    }
}
