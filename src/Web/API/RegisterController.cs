using System.Threading.Tasks;
using Mbrcld.Application.Features.Users.Commands;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.UAE;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mbrcld.Web.API
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/register")]
    public class RegisterController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUAEService uaeService;

        public RegisterController(IMediator mediator, IUAEService uaeService)
        {
            this.mediator = mediator;
            this.uaeService = uaeService;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        public async Task<ActionResult> RegisterNewUser(RegisterNewUserCommand command)
        {
            var result = await this.mediator.Send(command);
            return FromResult(result);
        }

        [HttpGet("uae/profile")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        public async Task<ActionResult> UAEProfle([FromQuery] string token)
        {
            var result = await this.uaeService.GetProfile(token);
            return Ok(result);
        }

        [HttpGet("uae/token")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        public async Task<ActionResult> UAEToken([FromQuery] string code, [FromQuery] string redirectUrl)
        {
            var result = await this.uaeService.GetToken(code, redirectUrl);
            return Ok(result);
        }
    }
}
