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
    [Route("api/mentor")]
    public class MentorsController : BaseController
    {
        private readonly IMediator mediator;

        public MentorsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "Alumni")]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetMentorById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var mentor = await this.mediator.Send(new GetMentorByIdQuery(id));
            mentor.PictureUrl = Url.RouteUrl("GetMentorProfilePicture", new { key = mentor.Id });
            return Ok(mentor);
        }
    }
}
