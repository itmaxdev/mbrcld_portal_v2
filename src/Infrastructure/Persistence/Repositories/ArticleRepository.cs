using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class ArticleRepository : IArticleRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public ArticleRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Maybe<Article>> CreateAsync(Article article, CancellationToken cancellationToken = default)
        {
            var odataArticle = this.mapper.Map<ODataArticle>(article);
            odataArticle.CreatedOn = DateTime.UtcNow;

            await webApiClient.For<ODataArticle>()
                .Set(odataArticle)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Article>(odataArticle);
        }

        public async Task UpdateAsync(Article article, CancellationToken cancellationToken = default)
        {
            var odataArticle = this.mapper.Map<ODataArticle>(article);

            await webApiClient.For<ODataArticle>()
                .Key(odataArticle)
                .Set(odataArticle)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<Article>> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataArticle = await webApiClient.For<ODataArticle>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            if(odataArticle != null)
            {
                var writtenbyid = odataArticle.Owner.SystemUserId;
                var writtenbyname = odataArticle.Owner.FullName;

                var article = this.mapper.Map<Article>(odataArticle);
                if (article.WrittenBy == Guid.Empty)
                {
                    article.WrittenBy = writtenbyid;
                    article.WrittenByName = writtenbyname;
                    article.AdminArticle = true;
                }
                return article;
            }
            return this.mapper.Map<Article>(odataArticle);
        }

        //public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        //{
        //    await webApiClient.For<ODataEnrollment>()
        //        .Key(id)
        //        .DeleteEntryAsync(cancellationToken)
        //        .ConfigureAwait(false);
        //}

        public async Task<IList<Article>> ListArticlesAsync(CancellationToken cancellationToken = default)
        {
            List<Article> articles = new List<Article> { };
            var odataArticles = await webApiClient.For<ODataArticle>()
                .Filter(c => c.ArticlesStatus == 3) //Published
                .OrderByDescending(x => x.Date)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var odataArticle in odataArticles)
            {
                var writtenbyid = odataArticle.Owner.SystemUserId;
                var writtenbyname = odataArticle.Owner.FullName;

                var article = this.mapper.Map<Article>(odataArticle);
                if (article.WrittenBy == Guid.Empty)
                {
                    article.WrittenBy = writtenbyid;
                    article.WrittenByName = writtenbyname;
                    article.AdminArticle = true;
                }
                articles.Add(article);
            }
            return articles;
        }

        public async Task<IList<Article>> ListUserArticlesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataArticles = await webApiClient.For<ODataArticle>()
                .Filter(c => c.WrittenBy.ContactId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Article>>(odataArticles);
        }

        public async Task<IList<Article>> SearchArticlesAsync(string search, CancellationToken cancellationToken = default)
        {
            List<Article> articles = new List<Article> { };
            var odataArticles = await webApiClient.For<ODataArticle>()
                .Filter(c => c.Name.Contains(search) || c.Desription.Contains(search))
                .Filter(c => c.ArticlesStatus == 3) //Published
                .OrderByDescending(x => x.Date)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var odataArticle in odataArticles)
            {
                var writtenbyid = odataArticle.Owner.SystemUserId;
                var writtenbyname = odataArticle.Owner.FullName;

                var article = this.mapper.Map<Article>(odataArticle);
                if (article.WrittenBy == Guid.Empty)
                {
                    article.WrittenBy = writtenbyid;
                    article.WrittenByName = writtenbyname;
                    article.AdminArticle = true;
                }
                articles.Add(article);
            }
            return articles;
        }

        public async Task<IList<Article>> SearchUserArticlesAsync(Guid userId, string search, CancellationToken cancellationToken = default)
        {
            var odataArticles = await webApiClient.For<ODataArticle>()
                .Filter(c => c.Name.Contains(search) || c.Desription.Contains(search))
                .Filter(c => c.WrittenBy.ContactId == userId)
                .OrderByDescending(x => x.Date)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Article>>(odataArticles);
        }
    }
}