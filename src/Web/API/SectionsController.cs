using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.ProgramQuestions.Queries;
using Mbrcld.Application.Features.Users.Commands;
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
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/sections")]
    public class SectionsController : BaseController
    {
        private readonly IMediator mediator;
        public SectionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        [Route("material-sections/{materialId}")]
        public async Task<ActionResult<IList<ListSectionsByMaterialIdViewModel>>> GetMaterialSections([FromRoute] Guid materialId)
        {
            var userId = User.GetUserId();
            var sections = await this.mediator.Send(new ListSectionsByMaterialIdQuery(materialId, userId));
            return Ok(sections);
        }

        [Authorize]
        [HttpGet]
        [Route("cohort-sections/{materialId}")]
        public async Task<ActionResult<IList<ListSectionsByCohortMaterialIdViewModel>>> GetCohortMaterialSections([FromRoute] Guid materialId)
        {
            var sections = await this.mediator.Send(new ListSectionsByCohortMaterialIdQuery(materialId));
            return Ok(sections);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetSectionByIdViewModel>> GetSection([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var section = await this.mediator.Send(new GetSectionByIdQuery(id, userId));
            return Ok(section);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpPost]
        public async Task<ActionResult> AddOrEditSection(Guid sectionId, Guid materialId, string name, string name_ar, int duration, decimal order, DateTime? startdate, DateTime? publishdate, MaterialStatuses sectionstatus, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new AddOrEditSectionCommand(
                userId: userId,
                sectionId: sectionId,
                materialId: materialId,
                name: name,
                name_ar: name_ar,
                duration: duration,
                order: order,
                startdate: startdate,
                publishdate: publishdate,
                sectionstatus: (int)sectionstatus
                );

            var result = await this.mediator.Send(command, cancellationToken);
            return FromResult(result);
        }

        [Authorize(Roles = "Applicant, Alumni")]
        [HttpPut]
        public async Task<ActionResult> EditSectionStatus(Guid sectionId, int status, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new UpdateSectionStatusCommand(
                userId: userId,
                sectionId: sectionId,
                status: status
                );

            var result = await this.mediator.Send(command, cancellationToken);
            return FromResult(result);
        }
    }
}
