using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.Users.Commands;
using Mbrcld.Application.Features.Users.Queries;
using Mbrcld.Application.Interfaces;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/project-ideas")]
    public class ProjectIdeasController : BaseController
    {
        private readonly IMediator mediator;

        public ProjectIdeasController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IList<ListProjectIdeasViewModel>>> GetProjectIdeas()
        {
            var projectIdeas = await this.mediator.Send(new ListProjectIdeasQuery());
            foreach (var projectIdea in projectIdeas)
            {
                projectIdea.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = projectIdea.Id });
            }
            return Ok(projectIdeas);
        }

        [Authorize(Roles = "Applicant, Alumni")]
        [HttpGet]
        [Route("user-project-ideas")]
        public async Task<ActionResult<IList<ListUserProjectIdeasViewModel>>> GetUserProjectIdeas()
        {
            var userId = User.GetUserId();
            var projectIdeas = await this.mediator.Send(new ListUserProjectIdeasQuery(userId));
            foreach (var projectIdea in projectIdeas)
            {
                projectIdea.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = projectIdea.Id });
            }
            return Ok(projectIdeas);
        }

        [Authorize]
        [HttpGet("search/{text}", Name = "SearchProjectIdeas")]
        public async Task<ActionResult<IList<SearchProjectIdeasViewModel>>> SearchProjectIdeas([FromRoute] string text)
        {
            var projectIdeas = await this.mediator.Send(new SearchProjectIdeasQuery(text));
            foreach (var projectIdea in projectIdeas)
            {
                projectIdea.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = projectIdea.Id });
            }
            return Ok(projectIdeas);
        }


        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetProjectIdeaById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var projectIdea = await this.mediator.Send(new GetProjectIdeaByIdQuery(id));
            projectIdea.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = projectIdea.Id });
            return Ok(projectIdea);
        }


        [Authorize(Roles = "Applicant, Alumni")]
        [HttpPost]
        public async Task<ActionResult> AddOrEditProjectIdea(IFormFile file, [FromForm] Guid? projectIdeaId, [FromForm] string description, [FromForm] string name, [FromForm] string body, [FromForm] string benchmark, [FromForm] int projectIdeaStatus, [FromForm] decimal budget, [FromForm] Guid? sectorId, [FromForm] string otherSector)
        {
            using var ms = new MemoryStream();

            Guid projectIdea = Guid.Empty;
            Guid sector = Guid.Empty;

            if (projectIdeaId != null)
            {
                projectIdea = projectIdeaId.Value;
            }

            if (sectorId != null)
            {
                sector = sectorId.Value;
            }

            if (file != null)
            {
                file.CopyTo(ms);
                var command = new AddOrEditProjectIdeaCommand(
                content: ms.ToArray(),
                contentType: file.ContentType,
                fileName: file.FileName,
                userId: User.GetUserId(),
                projectIdeaId: projectIdea,
                desrciption: description,
                name: name,
                body: body,
                benchmark: benchmark,
                projectIdeaStatus: projectIdeaStatus,
                budget: budget,
                sectorId: sector,
                otherSector: otherSector
                );

                var result = await this.mediator.Send(command);
                return FromResult(result);
            }
            else
            {
                var command = new AddOrEditProjectIdeaCommand(
                content: null,
                contentType: null,
                fileName: null,
                userId: User.GetUserId(),
                projectIdeaId: projectIdea,
                desrciption: description,
                name: name,
                body: body,
                benchmark: benchmark,
                projectIdeaStatus: projectIdeaStatus,
                budget: budget,
                sectorId: sector,
                otherSector: otherSector
                );

                var result = await this.mediator.Send(command);
                return FromResult(result);
            }
        }
    }
}
