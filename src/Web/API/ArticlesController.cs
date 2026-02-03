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
    [Route("api/articles")]
    public class ArticlesController : BaseController
    {
        private readonly IUserProfilePictureService profilePictureService;
        private readonly IMediator mediator;

        public ArticlesController(IMediator mediator, IUserProfilePictureService profilePictureService)
        {
            this.mediator = mediator;
            this.profilePictureService = profilePictureService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IList<ListArticlesViewModel>>> GetArticles()
        {
            var userId = User.GetUserId();
            var articles = await this.mediator.Send(new ListArtcilesQuery(userId));
            foreach (var article in articles)
            {
                article.PictureUrl = Url.RouteUrl("GetAttachedSmallPicture", new { key = article.Id });
                if (article.AdminArticle == true)
                {
                    article.ProfilePictureUrl = Url.RouteUrl("GetSystemUserProfilePicture", new { key = article.WrittenBy });
                }
                else
                {
                    var profile = await this.mediator.Send(new GetUserProfileQuery(article.WrittenBy));
                    article.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = profile.Value.ProfilePictureUrl });
                }
            }
            return Ok(articles);
        }

        [Authorize(Roles = "Applicant, Alumni")]
        [HttpGet]
        [Route("user-articles")]
        public async Task<ActionResult<IList<ListUserArticlesViewModel>>> GetUserArticles()
        {
            var userId = User.GetUserId();
            var articles = await this.mediator.Send(new ListUserArticlesQuery(userId));
            foreach (var article in articles)
            {
                article.PictureUrl = Url.RouteUrl("GetAttachedSmallPicture", new { key = article.Id });
            }
            return Ok(articles);
        }

        [Authorize]
        [HttpGet("search/{text}", Name = "SearchArticles")]
        public async Task<ActionResult<IList<SearchArticlesViewModel>>> SearchArticles([FromRoute] string text)
        {
            var userId = User.GetUserId();
            var articles = await this.mediator.Send(new SearchArticlesQuery(text, userId));
            foreach (var article in articles)
            {
                article.PictureUrl = Url.RouteUrl("GetAttachedSmallPicture", new { key = article.Id });
                if (article.AdminArticle == true)
                {
                    article.ProfilePictureUrl = Url.RouteUrl("GetSystemUserProfilePicture", new { key = article.WrittenBy });
                }
                else
                {
                    var profile = await this.mediator.Send(new GetUserProfileQuery(article.WrittenBy));
                    article.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = profile.Value.ProfilePictureUrl });
                }
            }
            return Ok(articles);
        }

        [Authorize]
        [HttpGet("search-user-articles/{text}", Name = "SearchUserArticles")]
        public async Task<ActionResult<IList<SearchUserArticlesViewModel>>> SearchUserArticles([FromRoute] string text)
        {
            var userId = User.GetUserId();
            var articles = await this.mediator.Send(new SearchUserArticlesQuery(text, userId));
            foreach (var article in articles)
            {
                var profile = await this.mediator.Send(new GetUserProfileQuery(article.WrittenBy));
                article.PictureUrl = Url.RouteUrl("GetAttachedSmallPicture", new { key = article.Id });
                article.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = profile.Value.ProfilePictureUrl });
            }
            return Ok(articles);
        }


        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetArticleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var article = await this.mediator.Send(new GetArticleByIdQuery(id, userId));
            article.PictureUrl = Url.RouteUrl("GetAttachedLargePicture", new { key = article.Id });
            if (article.AdminArticle == true)
            {
                article.ProfilePictureUrl = Url.RouteUrl("GetSystemUserProfilePicture", new { key = article.WrittenBy });
            }
            else
            {
                var profile = await this.mediator.Send(new GetUserProfileQuery(article.WrittenBy));
                article.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = profile.Value.ProfilePictureUrl });
            }
            return Ok(article);
        }


        [Authorize(Roles = "Applicant, Alumni")]
        [HttpPost]
        public async Task<ActionResult> AddOrEditArticle(IFormFile orignalFile, IFormFile thumbnailFile, [FromForm] Guid? articleId, [FromForm] string description, [FromForm] string name, [FromForm] string theArticle, [FromForm] int articleStatus, [FromForm] DateTime? date)
        {
            var ms = new MemoryStream();

            Guid article = Guid.Empty;
            if (articleId != null)
            {
                article = articleId.Value;
            }
            byte[] orignalContent = null;
            string orignalContentType = null, orignalFileName = null;
            if (orignalFile != null)
            {
                orignalFile.CopyTo(ms);
                orignalContent = ms.ToArray();
                orignalContentType = orignalFile.ContentType;
                orignalFileName = $"Large-{orignalFile.FileName}";
            }

            byte[] thumbnailContent = null;
            string thumbnailContentType = null, thumbnailFileName = null;

            if (thumbnailFile != null)
            {
                ms = new MemoryStream();
                thumbnailFile.CopyTo(ms);
                thumbnailContent = ms.ToArray();
                thumbnailContentType = thumbnailFile.ContentType;
                thumbnailFileName = $"Small-{thumbnailFile.FileName}";
            }

            var command = new AddOrEditArticleCommand(
            orignalContent: orignalContent,
            orignalContentType: orignalContentType,
            orignalFileName: orignalFileName,
            thumbnailContent: thumbnailContent,
            thumbnailContentType: thumbnailContentType,
            thumbnailFileName: thumbnailFileName,
            userId: User.GetUserId(),
            articleId: article,
            desrciption: description,
            name: name,
            theArticle: theArticle,
            articleStatus: articleStatus,
            date: date
            );

            var result = await this.mediator.Send(command);
            return FromResult(result);
        }
    }
}
