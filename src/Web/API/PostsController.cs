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
    [Route("api/posts")]
    public class PostsController : BaseController
    {
        private readonly IMediator mediator;

        public PostsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IList<ListPostsViewModel>>> GetPosts()
        {
            var userId = User.GetUserId();
            var posts = await this.mediator.Send(new ListPostsQuery(userId));
            foreach (var post in posts)
            {
                if (post.PostType == 0) // Image
                {
                    post.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = post.Id });
                    if (post.Type == 1) //article
                    {
                        var profile = await this.mediator.Send(new GetUserProfileQuery(post.WrittenBy));
                        post.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = profile.Value.ProfilePictureUrl });
                    }
                }
            }
            return Ok(posts);
        }

        [Authorize]
        [HttpGet]
        [Route("pinned")]
        public async Task<ActionResult<PinnedPostsViewModel>> GetPinnedPosts()
        {
            
            var posts = await this.mediator.Send(new PinnedPostsQuery());
            
            return Ok(posts);
        }

        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetPostById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var post = await this.mediator.Send(new GetPostByIdQuery(id, userId));
            post.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = post.Id });
            return Ok(post);
        }


        [Authorize]
        [HttpGet("search/{text}", Name = "SearchPosts")]
        public async Task<ActionResult<IList<SearchArticlesViewModel>>> SearchPosts([FromRoute] string text)
        {
            var userId = User.GetUserId();
            var posts = await this.mediator.Send(new SearchPostsQuery(userId , text));
            foreach (var post in posts)
            {
                post.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = post.Id });
                if (post.Type == 1) //article
                {
                    if (post.AdminArticle == true)
                    {
                        post.ProfilePictureUrl = Url.RouteUrl("GetSystemUserProfilePicture", new { key = post.WrittenBy });
                    }
                    else
                    {
                        var profile = await this.mediator.Send(new GetUserProfileQuery(post.WrittenBy));
                        post.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = profile.Value.ProfilePictureUrl });
                    }
                }
            }
            return Ok(posts);
        }
    }
}
