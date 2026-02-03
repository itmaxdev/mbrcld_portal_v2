using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface INewsFeedRepository
    {
        Task<IList<NewsFeed>> ListNewsFeedsAsync(Guid moduleid,CancellationToken cancellationToken = default);
        Task<IList<NewsFeed>> ListUserNewsFeedsAsync(Guid userId, Guid moduleid, CancellationToken cancellationToken = default);
        Task<Maybe<NewsFeed>> GetNewsFeedByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(NewsFeed newsfeed, CancellationToken cancellationToken = default);
        Task<Maybe<NewsFeed>> CreateAsync(NewsFeed newsfeed, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
