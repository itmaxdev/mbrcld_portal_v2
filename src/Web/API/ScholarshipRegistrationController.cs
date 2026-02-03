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
using Mbrcld.Application.Features.Scholarships.Command;
using Mbrcld.Application.Features.ScholarshipRegistrations.Queries;

namespace Mbrcld.Web.API
{
    [Authorize(Roles = "Applicant , Registrant, Alumni")]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/scholarshipregistration")]
    public class ScholarshipRegistrationController : BaseController
    {
        private readonly IMediator mediator;

        public ScholarshipRegistrationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserScholarshipRegistrationViewModel>>> GetScholarshipRegistration()
        {
            var userId = User.GetUserId();
            var scholarshipRegistration = await mediator.Send(new ListUserScholarshipRegistrationQuery(userId)).ConfigureAwait(false);
            if (scholarshipRegistration == null || !scholarshipRegistration.Any())
            {
                return NotFound("No scholarship registrations found for the user.");
            }

            return Ok(scholarshipRegistration);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddScholarshipRegistration(Guid scholarshipId)
        {
            var command = new AddScholarshipRegistrationCommand
            {
                ScholarshipId = scholarshipId,
                Id = User.GetUserId()
            };

            var result = await this.mediator.Send(command);
            return FromResult(result);
        }
    }

}