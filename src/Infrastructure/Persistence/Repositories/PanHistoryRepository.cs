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
    internal sealed class PanHistoryRepository : IPanHistoryRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public PanHistoryRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(PanHistory panHistory, CancellationToken cancellationToken = default)
        {
            var odataPanHistory = this.mapper.Map<ODataPanHistory>(panHistory);
            //odataPanHistory.ActionDate = DateTime.UtcNow;
            if (panHistory.ArticleId != Guid.Empty)
            {
                odataPanHistory.PostId = null;
                odataPanHistory.NewsFeedId = null;
                await webApiClient.For<ODataPanHistory>()
                    .Set(odataPanHistory)
                    .InsertEntryAsync(false, cancellationToken)
                    .ConfigureAwait(false);

                return Result.Success("Liked");
            }
            else if (panHistory.PostId != Guid.Empty)
            {
                odataPanHistory.ArticleId = null;
                odataPanHistory.NewsFeedId = null;
                await webApiClient.For<ODataPanHistory>()
                    .Set(odataPanHistory)
                    .InsertEntryAsync(false, cancellationToken)
                    .ConfigureAwait(false);

                return Result.Success("Liked");
            }
            else if (panHistory.NewsFeedId != Guid.Empty)
            {
                odataPanHistory.ArticleId = null;
                odataPanHistory.PostId = null;
                await webApiClient.For<ODataPanHistory>()
                    .Set(odataPanHistory)
                    .InsertEntryAsync(false, cancellationToken)
                    .ConfigureAwait(false);

                return Result.Success("Liked");
            }
            return Result.Failure("Error While Adding Like");
        }

        public async Task<Result> DeleteAsync(Guid ContactId, Guid? ArticleId, Guid? PostId, Guid? NewsFeedId, CancellationToken cancellationToken = default)
        {
            if (ArticleId != Guid.Empty)
            {
                await this.webApiClient.For<ODataPanHistory>()
                    .Filter(e => e.ArticleId.Id == ArticleId && e.ContactId.ContactId == ContactId && e.Action == 1)
                    .DeleteEntryAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Result.Success("Unliked");
            }
            else if (PostId != Guid.Empty)
            {
                await this.webApiClient.For<ODataPanHistory>()
                    .Filter(e => e.PostId.Id == PostId && e.ContactId.ContactId == ContactId && e.Action == 1)
                    .DeleteEntryAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Result.Success("Unliked");
            }
            else if (NewsFeedId != Guid.Empty)
            {
                await this.webApiClient.For<ODataPanHistory>()
                    .Filter(e => e.NewsFeedId.Id == NewsFeedId && e.ContactId.ContactId == ContactId && e.Action == 1)
                    .DeleteEntryAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Result.Success("Unliked");
            }
            return Result.Failure("An Error Occured when Unlike");
        }

        //public async Task<Maybe<PanHistory>> GetPanHistoryByUserIdAndArticleIdAsync(Guid id, Guid articleid, CancellationToken cancellationToken = default)
        //{
        //    var odataPanHistory = await webApiClient.For<ODataPanHistory>()
        //        .Filter(c => c.ContactId.ContactId == id)
        //        .Filter(e => e.ArticleId.Id == articleid)
        //        .Filter(e => e.Action == 1)
        //        .ProjectToModel()
        //        .FindEntryAsync(cancellationToken)
        //        .ConfigureAwait(false);

        //    return this.mapper.Map<PanHistory>(odataPanHistory);
        //}

        public async Task<IList<PanHistory>> ListPanHistoriesByArticlesAsync(Guid articleid, CancellationToken cancellationToken = default)
        {
            var odataPanHistory = await this.webApiClient.For<ODataPanHistory>()
                .Filter(e => e.ArticleId.Id == articleid)
                .Filter(e => e.Action == 1) //Like
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<PanHistory>>(odataPanHistory);
        }

        public async Task<IList<PanHistory>> ListPanHistoriesByNewsFeedsAsync(Guid newsfeedid, CancellationToken cancellationToken = default)
        {
            var odataPanHistory = await this.webApiClient.For<ODataPanHistory>()
                .Filter(e => e.NewsFeedId.Id == newsfeedid)
                .Filter(e => e.Action == 1) //Like
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<PanHistory>>(odataPanHistory);
        }

        public async Task<IList<PanHistory>> ListPanHistoriesByPostsAsync(Guid postid, CancellationToken cancellationToken = default)
        {
            var odataPanHistory = await this.webApiClient.For<ODataPanHistory>()
                .Filter(e => e.PostId.Id == postid)
                .Filter(e => e.Action == 1) //Like
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<PanHistory>>(odataPanHistory);
        }

        public async Task<IList<PanHistory>> CheckIfPanHistoriesosLikedAsync(Guid postid,Guid userid, CancellationToken cancellationToken = default)
        {
            var odataPanHistory =await this.webApiClient.For<ODataPanHistory>()
                .Filter(e => e.PostId.Id == postid)
                .Filter(e => e.ContactId.ContactId == userid)
                .Filter(e => e.Action == 1) //Like
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false) ;

            return this.mapper.Map<IList<PanHistory>>(odataPanHistory);
        }
    
        public async Task<IList<PanHistory>> ListPanHistoriesCommentsByPostsAsync(Guid postid, CancellationToken cancellationToken = default)
        {
            var odataPanHistory = await this.webApiClient.For<ODataPanHistory>()
                .Filter(e => e.PostId.Id == postid)
                .Filter(e => e.Action == 2) //Comment
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<PanHistory>>(odataPanHistory);
        }
        public async Task<IList<PanHistory>> ListPanHistoriesCommentsByArticlesAsync(Guid articleid, CancellationToken cancellationToken = default)
        {
            var odataPanHistory = await this.webApiClient.For<ODataPanHistory>()
                .Filter(e => e.ArticleId.Id == articleid)
                .Filter(e => e.Action == 2) //Comment
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<PanHistory>>(odataPanHistory);
        }

        public async Task<IList<PanHistory>> ListPanHistoriesCommentsByNewsFeedsAsync(Guid newsfeedid, CancellationToken cancellationToken = default)
        {
            var odataPanHistory = await this.webApiClient.For<ODataPanHistory>()
                .Filter(e => e.NewsFeedId.Id == newsfeedid)
                .Filter(e => e.Action == 2) //Comment
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<PanHistory>>(odataPanHistory);
        }
        public async Task<Result> DeleteCommentAsync(Guid ContactId, Guid? ArticleId, Guid? PostId, Guid? NewsFeedId, CancellationToken cancellationToken = default)
        {
            if (ArticleId != Guid.Empty)
            {
                await this.webApiClient.For<ODataPanHistory>()
                    .Filter(e => e.ArticleId.Id == ArticleId && e.ContactId.ContactId == ContactId && e.Action == 2)
                    .DeleteEntryAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Result.Success("Comment is deleted");
            }
            else if (PostId != Guid.Empty)
            {
                await this.webApiClient.For<ODataPanHistory>()
                    .Filter(e => e.PostId.Id == PostId && e.ContactId.ContactId == ContactId && e.Action == 2)
                    .DeleteEntryAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Result.Success("Comment is deleted");
            }
            else if (NewsFeedId != Guid.Empty)
            {
                await this.webApiClient.For<ODataPanHistory>()
                    .Filter(e => e.NewsFeedId.Id == NewsFeedId && e.ContactId.ContactId == ContactId && e.Action == 2)
                    .DeleteEntryAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Result.Success("Comment is deleted");
            }
            return Result.Failure("An Error Occured deleting the comment");
        }
    }
}
