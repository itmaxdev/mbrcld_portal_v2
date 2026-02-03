using Mbrcld.Application.Features.EducationQualifications.Commands;
using Mbrcld.Application.Features.EducationQualifications.Queries;
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
    [Route("api/profile/education-qualifications")]
    public class EducationQualificationsController : BaseController
    {
        private readonly IMediator mediator;

        public EducationQualificationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserEducationQualificationsViewModel>>> GetEducationQualifications()
        {
            var userId = User.GetUserId();
            var qualification = await mediator.Send(new ListUserEducationQualificationsQuery(userId)).ConfigureAwait(false);
            return Ok(qualification);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddEducationQualification(AddEducationQualificationCommand command)
        {
            command.ContactId = User.GetUserId();
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditEducationQualification([FromRoute] Guid id, EditEducationQualificationCommand command)
        {
            command.Id = id;
            await mediator.Send(command).ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> RemoveEducationQualification([FromRoute] Guid id)
        {
            await mediator.Send(new RemoveEducationQualificationCommand(id)).ConfigureAwait(false);
            return Ok();
        }
    }
}
