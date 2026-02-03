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
    [Route("api/university")]
    public class UniversitiesController : BaseController
    {
        private readonly IMediator mediator;

        public UniversitiesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetUniversityById([FromRoute] Guid id,  CancellationToken cancellationToken)
        {
            var university = await this.mediator.Send(new GetUniversityByIdQuery(id));
            return Ok(university);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<ActionResult<Guid>> UpdateUniversity(UpdateUniversityCommand command)
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

    }
}
