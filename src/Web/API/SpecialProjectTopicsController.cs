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
    [Authorize(Roles = "Alumni")]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/special-project-topics")]
    public class SpecialProjectTopicsController : ControllerBase
    {
        private readonly IMediator mediator;

        public SpecialProjectTopicsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ListSpecialProjectTopicsViewModel>>> ListSpecialProjectTopics()
        {
            var topics = await this.mediator.Send(new ListSpecialProjectTopicsQuery());
            return Ok(topics);
        }
    }
}
