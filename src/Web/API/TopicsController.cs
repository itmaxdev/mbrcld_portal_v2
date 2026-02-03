using Mbrcld.Application.Features.Metadata.Queries;
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
    [Authorize(Roles = "Applicant, Alumni")]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/topics")]
    public class TopicsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TopicsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{programId}")]
        public async Task<ActionResult<IList<ListTopicsByProgramIdViewModel>>> ListTopics([FromRoute] Guid programId)
        {
            var topics = await this.mediator.Send(new ListTopicsByProgramIdQuery(programId));
            return Ok(topics);
        }
    }
}
