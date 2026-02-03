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
    [Route("api/elite-membership-attendance")]
    public class EliteMembershipAttendanceController : BaseController
    {
        private readonly IMediator mediator;

        public EliteMembershipAttendanceController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "Alumni")]
        [HttpGet]
        public async Task<ActionResult> GetEliteMembershipAttendance(Guid eliteClubId, int membershipType, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var eliteClub = await this.mediator.Send(new GetEliteMembershipAttendanceQuery(userId, eliteClubId, membershipType));
            return Ok(eliteClub);
        }

        [Authorize(Roles = "Alumni")]
        [HttpPost]
        public async Task<ActionResult<Guid>> UpdateEliteMembershipAttendanceStatus(UpdateMembershipAttendanceStatusCommand command)
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

    }
}
