using Mbrcld.Application.Features.Achievements.Commands;
using Mbrcld.Application.Features.Achievements.Queries;
using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Domain.Entities;
using Mbrcld.Web.Constants;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/pan-history")]
    public class PanHistoriesController : BaseController
    {
        private readonly IMediator mediator;

        public PanHistoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("like")]
        public async Task<ActionResult<Guid>> AddAndRemoveLike(Guid ArticleId, Guid PostId, Guid NewsFeedId, bool Like)
        {
            var command = new AddorRemoveLikeCommand();
            command.ContactId = User.GetUserId();
            command.ArticleId = ArticleId;
            command.PostId = PostId;
            command.NewsFeedId = NewsFeedId;
            command.Like = Like;

            var result = await mediator.Send(command).ConfigureAwait(false);

            return FromResult(result);
        }

        [HttpPost]
        [Route("comment")]
        public async Task<ActionResult<Guid>> AddComment(Guid ArticleId, Guid postId, Guid newsfeedId, string comment)
        {
            var command = new AddCommentCommand();
            command.ContactId = User.GetUserId();
            command.ArticleId = ArticleId;
            command.PostId = postId;
            command.NewsFeedId = newsfeedId;
            command.Comment = comment;

            var result = await mediator.Send(command).ConfigureAwait(false);

            return FromResult(result);
        }

        [Authorize(Roles = "Applicant, Alumni")]
        [HttpDelete]
        [Route("delete-comment")]
        public async Task<ActionResult<Guid>> DeleteComment(Guid ArticleId, Guid postId, Guid newsfeedId)
        {
            var command = new DeleteCommentCommand();
            command.ContactId = User.GetUserId();
            command.ArticleId = ArticleId;
            command.PostId = postId;
            command.NewsFeedId = newsfeedId;

            var result = await mediator.Send(command).ConfigureAwait(false);

            return FromResult(result);
        }

        [HttpGet]
        [Route("post-comments/{id}")]
        public async Task<ActionResult<IList<ListCommentsByPostIdViewModel>>> ListCommentPanHistory([FromRoute] Guid id)
        {
            var comments = await this.mediator.Send(new ListCommentsByPostIdQuery(id));
            return Ok(comments);
        }

        [HttpGet]
        [Route("newsfeed-comments/{id}")]
        public async Task<ActionResult<IList<ListCommentsByNewsFeedIdViewModel>>> ListNewsFeedCommentPanHistory([FromRoute] Guid id)
        {
            var comments = await this.mediator.Send(new ListCommentsByNewsFeedIdQuery(id));
            return Ok(comments);
        }

        [HttpGet]
        [Route("article-comments/{id}")]
        public async Task<ActionResult<IList<ListCommentsByArticleIdViewModel>>> ListArticleCommentPanHistory([FromRoute] Guid id)
        {
            var comments = await this.mediator.Send(new ListCommentsByArticleIdQuery(id));
            return Ok(comments);
        }

        //[HttpDelete]
        //[Route("{id}")]
        //public async Task<ActionResult> RemoveAchievement([FromRoute] Guid id)
        //{
        //    var result = await mediator.Send(new RemoveAchievementCommand(id)).ConfigureAwait(false);
        //    return FromResult(result);
        //}
    }
}
