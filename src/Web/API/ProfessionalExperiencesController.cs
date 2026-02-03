using AutoMapper.Internal;
using Mbrcld.Application.Features.ProfessionalExperiences.Commands;
using Mbrcld.Application.Features.ProfessionalExperiences.Queries;
using Mbrcld.Web.Constants;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AutoMapper.Internal.ExpressionFactory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mbrcld.Web.API
{
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/profile/professional-experiences")]
    public class ProfessionalExperiencesController : BaseController
    {
        private readonly IMediator mediator;

        public ProfessionalExperiencesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserProfessionalExperiencesViewModel>>> GetProfessionalExperience()
        {
            var userId = User.GetUserId();
            var profExp = await mediator.Send(new ListUserProfessionalExperiencesQuery(userId)).ConfigureAwait(false);
            profExp.ForAll(item => {
                item.Completed = !string.IsNullOrEmpty(item.JobTitle) &&
                !string.IsNullOrEmpty(item.OrganizationName) &&
                item.From != default(DateTime) &&
                (item.To == null || item.To != default(DateTime)) &&
                item.Industry != Guid.Empty &&
                item.Sector != Guid.Empty &&
                item.OrganizationSize != null &&
                item.PositionLevel != null &&
                item.OrganizationLevel != null;

            });
            return Ok(profExp);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateProfessionalExperience(AddProfessionalExperienceCommand command)
        {
            command.ContactId = User.GetUserId();
            var result = await mediator.Send(command).ConfigureAwait(false);
            command.Completed = !string.IsNullOrEmpty(command.JobTitle) &&
                                 !string.IsNullOrEmpty(command.OrganizationName) &&
                                 command.From != default(DateTime) &&
                                 (command.To == null || command.To != default(DateTime)) &&
                                 command.Industry != Guid.Empty &&
                                 command.Sector != Guid.Empty &&
                                 command.OrganizationSize != null &&
                                 command.PositionLevel != null &&
                                 command.OrganizationLevel != null;
            return FromResult(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateProfessionalExperience([FromRoute] Guid id, EditProfessionalExperienceCommand command)
        {
            command.Id = id;
            await mediator.Send(command).ConfigureAwait(false);
            command.Completed = !string.IsNullOrEmpty(command.JobTitle) &&
                     !string.IsNullOrEmpty(command.OrganizationName) &&
                     command.From != default(DateTime) &&
                     (command.To == null || command.To != default(DateTime)) &&
                     command.Industry != Guid.Empty &&
                     command.Sector != Guid.Empty &&
                     command.OrganizationSize != null &&
                     command.PositionLevel != null &&
                     command.OrganizationLevel != null;

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteProfessionalExperience([FromRoute] Guid id)
        {
            await mediator.Send(new RemoveProfessionalExperienceCommand(id)).ConfigureAwait(false);
            return Ok();
        }
    }
}
