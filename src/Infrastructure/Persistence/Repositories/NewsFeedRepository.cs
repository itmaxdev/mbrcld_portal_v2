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
    internal sealed class NewsFeedRepository : INewsFeedRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public NewsFeedRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Maybe<NewsFeed>> CreateAsync(NewsFeed newsfeed, CancellationToken cancellationToken = default)
        {
            var odataNewsFeed = this.mapper.Map<ODataNewsFeed>(newsfeed);

            await webApiClient.For<ODataNewsFeed>()
                .Set(odataNewsFeed)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<NewsFeed>(odataNewsFeed);
        }

        public async Task UpdateAsync(NewsFeed newsfeed, CancellationToken cancellationToken = default)
        {
            var odataNewsFeed = this.mapper.Map<ODataNewsFeed>(newsfeed);

            await webApiClient.For<ODataNewsFeed>()
                .Key(odataNewsFeed)
                .Set(odataNewsFeed)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<NewsFeed>> GetNewsFeedByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataNewsFeed = await webApiClient.For<ODataNewsFeed>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<NewsFeed>(odataNewsFeed);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await webApiClient.For<ODataNewsFeed>()
                .Key(id)
                .DeleteEntryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IList<NewsFeed>> ListNewsFeedsAsync(Guid moduleid, CancellationToken cancellationToken = default)
        {
            var odataNewsFeeds = await webApiClient.For<ODataNewsFeed>()
                .Filter(c => c.Module.Id == moduleid)
                .Filter(c => c.Status == 1) //Published
                .OrderByDescending(x => x.Order)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<NewsFeed>>(odataNewsFeeds);
        }

        public async Task<IList<NewsFeed>> ListUserNewsFeedsAsync(Guid userId, Guid moduleid, CancellationToken cancellationToken = default)
        {
            var odataNewsFeeds = await webApiClient.For<ODataNewsFeed>()
                .Filter(c => c.Instructor.ContactId == userId)
                .Filter(c => c.Module.Id == moduleid)
                .OrderByDescending(x => x.Order)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<NewsFeed>>(odataNewsFeeds);
        }
    }
}