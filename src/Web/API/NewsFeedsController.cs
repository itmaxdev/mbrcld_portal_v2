using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.NewsFeeds.Commands;
using Mbrcld.Application.Features.TrainingCourses.Commands;
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
    [Route("api/newsfeeds")]
    public class NewsFeedsController : BaseController
    {
        private readonly IUserProfilePictureService profilePictureService;
        private readonly IMediator mediator;

        public NewsFeedsController(IMediator mediator, IUserProfilePictureService profilePictureService)
        {
            this.mediator = mediator;
            this.profilePictureService = profilePictureService;
        }

        [Authorize(Roles = "Applicant, Alumni")]
        [HttpGet]
        [Route("module-newsfeeds/{moduleid}")]
        public async Task<ActionResult<IList<ListNewsFeedsViewModel>>> GetNewsFeeds([FromRoute] Guid moduleid)
        {
            var userId = User.GetUserId();
            var newsfeeds = await this.mediator.Send(new ListNewsFeedsQuery(userId, moduleid));
            foreach (var newsfeed in newsfeeds)
            {
                var profile = await this.mediator.Send(new GetUserProfileQuery(newsfeed.InstructorId));
                newsfeed.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = profile.Value.ProfilePictureUrl });
                newsfeed.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = newsfeed.Id });
            }
            return Ok(newsfeeds);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpGet]
        [Route("instructorOrAdmin-newsfeeds/{moduleid}")]
        public async Task<ActionResult<IList<ListUserNewsFeedsViewModel>>> GetInstructorOrAdminNewsFeeds([FromRoute] Guid moduleid)
        {
            var userId = User.GetUserId();
            var newsfeeds = await this.mediator.Send(new ListUserNewsFeedsQuery(userId, moduleid));
            foreach (var newsfeed in newsfeeds)
            {
                var profile = await this.mediator.Send(new GetUserProfileQuery(newsfeed.InstructorId));
                newsfeed.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = profile.Value.ProfilePictureUrl });
                newsfeed.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = newsfeed.Id });
            }
            return Ok(newsfeeds);
        }


        [Authorize(Roles = "Instructor, Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> NewsFeedById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var newsfeed = await this.mediator.Send(new GetNewsFeedByIdQuery(id, userId));
            var profile = await this.mediator.Send(new GetUserProfileQuery(newsfeed.InstructorId));
            newsfeed.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = profile.Value.ProfilePictureUrl });
            newsfeed.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = newsfeed.Id });
            return Ok(newsfeed);
        }


        [Authorize(Roles = "Instructor, Admin")]
        [HttpPost]
        public async Task<ActionResult> AddOrEditNewsFeed(IFormFile file, [FromForm] Guid moduleid, [FromForm] Guid? newsfeedid,
            [FromForm] string name, [FromForm] int duration, [FromForm] decimal order, [FromForm] int type, [FromForm] string text, [FromForm] string url,
            [FromForm] int status, [FromForm] bool notifyusers, [FromForm] DateTime? meetingstartdate, [FromForm] DateTime? publishdate, [FromForm] DateTime? expirydate)
        {
            var userId = User.GetUserId();
            using var ms = new MemoryStream();

            Guid newsfeed = Guid.Empty;
            if (newsfeedid != null)
            {
                newsfeed = newsfeedid.Value;
            }

            if (file != null)
            {
                file.CopyTo(ms);
                var command = new AddOrEditNewsFeedCommand(
                 doccontent: ms.ToArray(),
                 contentType: file.ContentType,
                 fileName: file.FileName,
                 newsfeedid: newsfeed,
                 moduleid: moduleid,
                 instructorid: userId,
                 name: name,
                 duration: duration,
                 order: order,
                 type: type, // 1:Text 2:Video 3:Doc 4:Meeting 5:Sticky Note
                 text: text,
                 url: url,
                 status: status,
                 notifyusers: notifyusers,
                 meetingstartdate: meetingstartdate,
                 expirydate: expirydate,
                 publishdate: publishdate
                 );

                var result = await this.mediator.Send(command);
                return FromResult(result);
            }
            else
            {
                var command = new AddOrEditNewsFeedCommand(
                 doccontent: null,
                 contentType: null,
                 fileName: null,
                 newsfeedid: newsfeed,
                 moduleid: moduleid,
                 instructorid: userId,
                 name: name,
                 duration: duration,
                 order: order,
                 type: type, // 1:Text 2:Video 3:Doc 4:Meeting 5:Sticky Note
                 text: text,
                 url: url,
                 status: status,
                 notifyusers: notifyusers,
                 meetingstartdate: meetingstartdate,
                 expirydate: expirydate,
                 publishdate: publishdate
                    );

                var result = await this.mediator.Send(command);
                return FromResult(result);
            }
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteContent([FromRoute] Guid id)
        {
            var command = new RemoveNewsFeedCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }
    }
}
