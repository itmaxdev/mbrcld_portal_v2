using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.Modules.Commands;
using Mbrcld.Application.Features.ProgramQuestions.Queries;
using Mbrcld.Web.Constants;
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
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/modules")]
    public class ModulesController : BaseController
    {
        private readonly IMediator mediator;
        public ModulesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("program-modules/{programId}")]
        public async Task<ActionResult<IList<ListModulesByProgramIdViewModel>>> GetProgramModules([FromRoute] Guid programId)
        {
            var userId = User.GetUserId();
            var modules = await this.mediator.Send(new ListModulesByProgramIdQuery(programId, userId));
            return Ok(modules);
        }

        [HttpGet]
        [Route("elite-club-modules/{eliteClubId}")]
        public async Task<ActionResult<IList<ListModulesByEliteClubIdViewModel>>> GetEliteClubModules([FromRoute] Guid eliteClubId)
        {
            var userId = User.GetUserId();
            var modules = await this.mediator.Send(new ListModulesByEliteClubIdQuery(eliteClubId, userId));
            return Ok(modules);
        }

        [HttpGet]
        [Route("cohort-modules/{cohortId}")]
        public async Task<ActionResult<IList<ListModulesByCohortIdViewModel>>> GetCohortModules([FromRoute] Guid cohortId)
        {
            var modules = await this.mediator.Send(new ListModulesByCohortIdQuery(cohortId));
            return Ok(modules);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetModuleByIdViewModel>> GetModule([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var module = await this.mediator.Send(new GetModuleByIdQuery(id));
            return Ok(module);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpPut]
        [Route("module-overview")]
        public async Task<ActionResult<Guid>> UpdateInstructorProject(UpdateOverviewCommand command)
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }


        [Authorize(Roles = "Applicant, Alumni")]
        [HttpGet]
        [Route("university-profile/{moduleId}")]
        public async Task<ActionResult<GetUniversityProfileViewModel>> GetUniversityProfile([FromRoute] Guid moduleId)
        {
            var userId = User.GetUserId();
            var University = await this.mediator.Send(new GetUniversityProfileQuery(moduleId));
            return Ok(University);
        }

        [Authorize]
        [HttpGet]
        [Route("applicant-profile/{Id}")]
        public async Task<ActionResult<GetApplicantProfileViewModel>> GetApplicantProfile([FromRoute] Guid Id)
        {
            var Applicant = await this.mediator.Send(new GetApplicantProfileQuery(Id));
            Applicant.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = Applicant.ProfilePictureUrl });
            return Ok(Applicant);
        }

        [Authorize(Roles = "Applicant, Instructor, Alumni, Admin")]
        [HttpGet]
        [Route("module-applicants/{moduleId}")]
        public async Task<ActionResult<IList<ListModuleApplicantsViewModel>>> GetModuleApplicants([FromRoute] Guid moduleId)
        {
            var moduleApplicants = await this.mediator.Send(new ListModuleApplicantsQuery(moduleId));
            foreach (var moduleApplicant in moduleApplicants)
            {
                moduleApplicant.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = moduleApplicant.ProfilePictureUrl });
            }
            return Ok(moduleApplicants);
        }

    }
}
