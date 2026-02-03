using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.Scholarships.Queries;
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
    [Authorize(Roles = "Applicant , Registrant, Alumni")]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/scholarships")]
    public class ScholarshipController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IAttachedPictureService scholarshipPictureService;

        public ScholarshipController(IMediator mediator, IAttachedPictureService scholarshipPictureService)
        {
            this.mediator = mediator;
            this.scholarshipPictureService = scholarshipPictureService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ListScholarshipViewModel>>> GetScholarshipsAsync()
        {
            var scholarships = await mediator.Send(new ListScholarshipQuery());
            foreach (var scholarshiprec in scholarships)
            {
                scholarshiprec.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = scholarshiprec.Id });
            }

            return Ok(scholarships);
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetScholarshipById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var scholarships = await this.mediator.Send(new GetScholarshipByIdQuery(id, userId));
            if (scholarships != null)
            {
                scholarships.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = id });
            }
            return Ok(scholarships);
        }
    }
}
