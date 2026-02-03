using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.Panel.Queries;
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
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/panel")]
    public class RightPanelController : BaseController
    {
        private readonly IMediator mediator;

        public RightPanelController(IMediator mediator)
        {
            this.mediator = mediator;
        }      

        [HttpGet]
        [Route("pinned")]
        public async Task<ActionResult<PinnedPostsViewModel>> GetPinnedPosts()
        {
            var posts = await this.mediator.Send(new PinnedPostsQuery());
            return Ok(posts);
        }

        [Authorize(Roles = "Registrant")]
        [HttpGet]
        [Route("current-program")]
        public async Task<ActionResult<GetCurrentActiveProgramViewModel>> GetCurrentActiveProgram(CancellationToken cancellationToken)
        {
            var program = await this.mediator.Send(new GetCurrentActiveProgramQuery());
            if(program!=null)
            program.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = program.Id });
            return Ok(program);
        }

        [Authorize(Roles = "Applicant")]
        [Authorize]
        [HttpGet]
        [Route("current-module")]
        public async Task<ActionResult<GetCurrentModuleViewModel>> GetCurrentModule(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var module = await this.mediator.Send(new GetCurrentModuleQuery(userId));
            module.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = module.ProgramId });

            return Ok(module);
        }

        [HttpGet]
        [Route("lists")]
        public async Task<ActionResult<Panel3ViewModel>> GetPanelList()
        {
            var posts = await this.mediator.Send(new Panel3Query());
            return Ok(posts);
        }
    }
}
