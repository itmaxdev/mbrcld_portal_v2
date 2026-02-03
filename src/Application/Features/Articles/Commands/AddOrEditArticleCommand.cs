using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class AddOrEditArticleCommand : IRequest<Result>
    {
        #region Command
        public Guid ArticleId { get; }
        public byte[] OrignalContent { get; }
        public string OrignalContentType { get; }
        public string OrignalFileName { get; }
        public byte[] ThumbnailContent { get; }
        public string ThumbnailContentType { get; }
        public string ThumbnailFileName { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TheArticle { get; set; }
        public int ArticleStatus { get; set; }
        public Guid WrittenBy { get; set; }
        public DateTime? Date { get; set; }
        public CancellationToken CancellationToken { get; }

        public AddOrEditArticleCommand(
            byte[]? orignalContent,
            string? orignalContentType,
            string? orignalFileName,
            byte[]? thumbnailContent,
            string? thumbnailContentType,
            string? thumbnailFileName,
            Guid articleId,
            Guid userId,
            string desrciption,
            string name,
            string theArticle,
            int articleStatus,
            DateTime? date
            )
        {
            this.ArticleId = articleId;
            this.WrittenBy = userId;
            this.OrignalContent = orignalContent;
            this.OrignalContentType = orignalContentType;
            this.OrignalFileName = orignalFileName;
            this.ThumbnailContent = thumbnailContent;
            this.ThumbnailContentType = thumbnailContentType;
            this.ThumbnailFileName = thumbnailFileName;
            this.Description = desrciption;
            this.Name = name;
            this.TheArticle = theArticle;
            this.ArticleStatus = articleStatus;
            this.Date = date;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<AddOrEditArticleCommand, Result>
        {
            private readonly IAttachedPictureService attachedPictureService;
            private readonly IArticleRepository articleRepository;

            public CommandHandler(IAttachedPictureService attachedPictureService, IArticleRepository articleRepository)
            {
                this.attachedPictureService = attachedPictureService;
                this.articleRepository = articleRepository;
            }

            public async Task<Result> Handle(AddOrEditArticleCommand request, CancellationToken cancellationToken)
            {
                var articleid = request.ArticleId;
                var article = await articleRepository.GetArticleByIdAsync(articleid, cancellationToken);
                if (article.HasValue)
                {
                    var articledata = article.Value;
                    articledata.Name = request.Name;
                    articledata.Description = request.Description;
                    articledata.TheArticle = request.TheArticle;
                    articledata.ArticleStatus = request.ArticleStatus;
                    articledata.WrittenBy = request.WrittenBy;
                    articledata.Date = request.Date;

                    await articleRepository.UpdateAsync(articledata).ConfigureAwait(false);
                }
                else
                {
                    var articledata = Article.Create(
                        description: request.Description,
                        theArticle: request.TheArticle,
                        articlestatus: request.ArticleStatus,
                        userid: request.WrittenBy,
                        date: request.Date,
                        name: request.Name
                    );

                    var returnedarticle = await articleRepository.CreateAsync(articledata).ConfigureAwait(false);
                    articleid = returnedarticle.Value.Id;
                }

                if (request.OrignalContent != null)
                {
                    await this.attachedPictureService.AddOrEditArticlePictureAsync(
                        articleid, request.OrignalContent, request.OrignalContentType,
                        request.OrignalFileName, cancellationToken);
                }
                if (request.ThumbnailContent != null)
                {
                    await this.attachedPictureService.AddOrEditArticlePictureAsync(
                        articleid, request.ThumbnailContent, request.ThumbnailContentType,
                        request.ThumbnailFileName, cancellationToken);
                }
                return Result.Success(articleid);
            }
        }
        #endregion
    }
}
