using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IPanHistoryRepository
    {
        Task<Result> CreateAsync(PanHistory panhistory, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(Guid contactId , Guid? ArticleId, Guid? PostId, Guid? NewsFeedId, CancellationToken cancellationToken = default);
        //Task<Maybe<PanHistory>> GetPanHistoryByUserIdAndArticleIdAsync(Guid userId, Guid articleid, CancellationToken cancellationToken = default);
        Task<IList<PanHistory>> ListPanHistoriesByArticlesAsync(Guid articleid, CancellationToken cancellationToken = default);
        Task<IList<PanHistory>> ListPanHistoriesByNewsFeedsAsync(Guid newsfeedid, CancellationToken cancellationToken = default);
        Task<IList<PanHistory>> ListPanHistoriesByPostsAsync(Guid postid, CancellationToken cancellationToken = default);
        Task<IList<PanHistory>> CheckIfPanHistoriesosLikedAsync(Guid postid,Guid userid, CancellationToken cancellationToken = default);
        Task<IList<PanHistory>> ListPanHistoriesCommentsByPostsAsync(Guid postid, CancellationToken cancellationToken = default);
        Task<IList<PanHistory>> ListPanHistoriesCommentsByArticlesAsync(Guid articleid, CancellationToken cancellationToken = default);
        Task<IList<PanHistory>> ListPanHistoriesCommentsByNewsFeedsAsync(Guid newsfeedid, CancellationToken cancellationToken = default);
        Task<Result> DeleteCommentAsync(Guid contactId, Guid? ArticleId, Guid? PostId, Guid? NewsFeedId, CancellationToken cancellationToken = default);

    }
}
