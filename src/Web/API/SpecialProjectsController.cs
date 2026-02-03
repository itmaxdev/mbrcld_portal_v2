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
    [Route("api/special-projects")]
    public class SpecialProjectsController : BaseController
    {
        private readonly IMediator mediator;

        public SpecialProjectsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        //[Authorize]
        //[HttpGet]
        //public async Task<ActionResult<IList<ListSpecialProjectsViewModel>>> GetSpecialProjects()
        //{
        //    var specialProjects = await this.mediator.Send(new ListSpecialProjectsQuery());
        //    foreach (var specialProject in specialProjects)
        //    {
        //        specialProject.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = specialProject.Id });
        //    }
        //    return Ok(specialProjects);
        //}

        [Authorize(Roles = "Alumni")]
        [HttpGet]
        [Route("user-special-projects")]
        public async Task<ActionResult<IList<ListUserSpecialProjectsViewModel>>> GetUserSpecialProjects()
        {
            var userId = User.GetUserId();
            var specialProjects = await this.mediator.Send(new ListUserSpecialProjectsQuery(userId));
            foreach (var specialProject in specialProjects)
            {
                specialProject.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = specialProject.Id });
            }
            return Ok(specialProjects);
        }

        //[Authorize]
        //[HttpGet("search/{text}", Name = "SearchSpecialProjects")]
        //public async Task<ActionResult<IList<SearchSpecialProjectsViewModel>>> SearchSpecialProjects([FromRoute] string text)
        //{
        //    var specialProjects = await this.mediator.Send(new SearchSpecialProjectsQuery(text));
        //    foreach (var specialProject in specialProjects)
        //    {
        //        specialProject.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = specialProject.Id });
        //    }
        //    return Ok(specialProjects);
        //}


        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetSpecialProjectById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var specialProject = await this.mediator.Send(new GetSpecialProjectByIdQuery(id));
            specialProject.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = specialProject.Id });
            return Ok(specialProject);
        }


        [Authorize(Roles = "Alumni")]
        [HttpPost]
        public async Task<ActionResult> AddOrEditSpecialProject(IFormFile file, [FromForm] Guid? specialProjectId, [FromForm] string description, [FromForm] string benchmark, [FromForm] string name, [FromForm] string body, [FromForm] int specialProjectStatus, [FromForm] Guid? specialProjectTopicId, [FromForm] decimal budget, [FromForm] Guid? sectorId, [FromForm] string otherSector)
        {
            using var ms = new MemoryStream();

            Guid specialProject = Guid.Empty;
            Guid specialProjectTopic = Guid.Empty;
            Guid sector = Guid.Empty;

            if (specialProjectId != null)
            {
                specialProject = specialProjectId.Value;
            }

            if (specialProjectTopicId != null)
            {
                specialProjectTopic = specialProjectTopicId.Value;
            }

            if (sectorId != null)
            {
                sector = sectorId.Value;
            }

            if (file != null)
            {
                file.CopyTo(ms);
                var command = new AddOrEditSpecialProjectCommand(
                content: ms.ToArray(),
                contentType: file.ContentType,
                fileName: file.FileName,
                userId: User.GetUserId(),
                specialProjectId: specialProject,
                desrciption: description,
                name: name,
                body: body,
                benchmark: benchmark,
                budget: budget,
                specialProjectStatus: specialProjectStatus,
                specialProjectTopicId: specialProjectTopic,
                sectorId: sector,
                otherSector: otherSector
                );

                var result = await this.mediator.Send(command);
                return FromResult(result);
            }
            else
            {
                var command = new AddOrEditSpecialProjectCommand(
                content: null,
                contentType: null,
                fileName: null,
                userId: User.GetUserId(),
                specialProjectId: specialProject,
                desrciption: description,
                name: name,
                body: body,
                benchmark: benchmark,
                budget: budget,
                specialProjectStatus: specialProjectStatus,
                specialProjectTopicId: specialProjectTopic,
                sectorId: sector,
                otherSector: otherSector
                );

                var result = await this.mediator.Send(command);
                return FromResult(result);
            }
        }
    }
}
