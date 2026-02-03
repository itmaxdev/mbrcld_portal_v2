using Mbrcld.Application.Features.LanguageSkills.Commands;
using Mbrcld.Application.Features.LanguageSkills.Queries;
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
    [Route("api/profile/language-skills")]
    public class LanguageSkillsController : BaseController
    {
        private readonly IMediator mediator;

        public LanguageSkillsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserLanguageSkillsViewModel>>> GetLanguageSkills()
        {
            var userId = User.GetUserId();
            var LanguageSkills = await this.mediator.Send(new ListUserLanguageSkillsQuery(userId));
            if (LanguageSkills is null)
            {
                return NotFound();
            }
            return Ok(LanguageSkills);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateLanguageSkill(AddLanguageSkillCommand command)
        {
            command.ContactId = User.GetUserId();
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateLanguageSkill([FromRoute] Guid id, EditLanguageSkillCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteLanguageSkill([FromRoute] Guid id)
        {
            var command = new RemoveLanguageSkillCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }
    }
}