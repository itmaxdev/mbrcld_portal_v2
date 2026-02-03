using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.Universities.Commands;
using Mbrcld.Application.Interfaces;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/eliteclub")]
    public class EliteClubController : BaseController
    {
        private readonly IMediator mediator;

        public EliteClubController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "Alumni")]
        [HttpGet]
        public async Task<ActionResult> GetAlumniActiveEliteClub()
        {
            var userId = User.GetUserId();
            var eliteClub = await this.mediator.Send(new GetAlumniEliteClubQuery(userId));
            return Ok(eliteClub);
        }

        [Authorize(Roles = "Alumni")]
        [HttpGet]
        [Route("overview/{id}")]
        public async Task<ActionResult> GetEliteClubOverviewById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var eliteClub = await this.mediator.Send(new GetEliteClubOverviewByIdQuery(id));
            return Ok(eliteClub);
        }

    }
}
