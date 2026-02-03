using Mbrcld.Application.Features.SkillsAndInterests.Commands;
using Mbrcld.Application.Features.SkillsAndInterests.Queries;
using Mbrcld.Web.Constants;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/profile/skills-and-interests")]
    public class SkillsAndInterestsController : BaseController
    {
        private readonly IMediator mediator;

        public SkillsAndInterestsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserSkillsAndInterestsViewModel>>> ListInterests()
        {
            var userId = User.GetUserId();
            var interests = await this.mediator.Send(new ListUserSkillsAndInterestsQuery(userId));
            return Ok(interests);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddInterest(AddInterestCommand command)
        {
            command.ContactId = User.GetUserId();
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditInterest([FromRoute] Guid id, EditInterestCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> RemoveInterest([FromRoute] Guid id)
        {
            var result = await mediator.Send(new RemoveInterestCommand(id)).ConfigureAwait(false);
            return FromResult(result);
        }
    }
}
