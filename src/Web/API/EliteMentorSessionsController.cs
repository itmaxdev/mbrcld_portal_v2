using Mbrcld.Application.Features.EliteMentorSessions.Commands;
using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.TeamMember.Commands;
using Mbrcld.Application.Features.Universities.Commands;
using Mbrcld.Application.Interfaces;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/elite-mentor-session")]
    public class EliteMentorSessionsController : BaseController
    {
        private readonly IMediator mediator;

        public EliteMentorSessionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "Alumni")]
        [HttpGet]
        public async Task<ActionResult<IList<ListEliteMentorSessionsViewModel>>> ListEliteMentorSessions()
        {
            var userId = User.GetUserId();
            var eliteMentorSessions = await this.mediator.Send(new ListEliteMentorSessionsByUserIdQuery(userId));
            foreach (var eliteMentorSession in eliteMentorSessions)
            {
                eliteMentorSession.MentorProfilePicture = Url.RouteUrl("GetMentorProfilePicture", new { key = eliteMentorSession.MentorId });
            }
            return Ok(eliteMentorSessions);
        }

        [Authorize(Roles = "Alumni")]
        [HttpPut]
        [Route("set-date")]
        public async Task<ActionResult<Guid>> SetEliteMentorSessionDate(UpdateEliteMentorSessionCommand command)
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }
    }
}
