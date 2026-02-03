using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListUserNewsFeedsQuery : IRequest<IList<ListUserNewsFeedsViewModel>>
    {
        #region Query
        public Guid UserId { get; }
        public Guid ModuleId { get; }

        public ListUserNewsFeedsQuery(Guid userid, Guid moduleid)
        {
            UserId = userid;
            ModuleId = moduleid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListUserNewsFeedsQuery, IList<ListUserNewsFeedsViewModel>>
        {
            private readonly INewsFeedRepository newsFeedRepository;
            private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(INewsFeedRepository newsFeedRepository, IPanHistoryRepository panHistoryRepository, IMapper mapper)
            {
                this.newsFeedRepository = newsFeedRepository;
                this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserNewsFeedsViewModel>> Handle(ListUserNewsFeedsQuery request, CancellationToken cancellationToken)
            {
                var newsfeeds = await newsFeedRepository.ListUserNewsFeedsAsync(request.UserId, request.ModuleId, cancellationToken);
                foreach (var newsfeed in newsfeeds)
                {
                    //var userlike = await panHistoryRepository.GetPanHistoryByUserIdAndArticleIdAsync(request.UserId, article.Id).ConfigureAwait(false);
                    var newsfeedlikes = await panHistoryRepository.ListPanHistoriesByNewsFeedsAsync(newsfeed.Id).ConfigureAwait(false);
                    foreach (var likes in newsfeedlikes)
                    {
                        if (likes.UserId == request.UserId)
                        {
                            newsfeed.Liked = true;
                            break;
                        }
                    }

                    if (newsfeedlikes.Count > 0)
                    {
                        newsfeed.Likes = newsfeedlikes.Count;
                    }
                }
                return mapper.Map<IEnumerable<ListUserNewsFeedsViewModel>>(newsfeeds).ToList();
            }
        }
        #endregion
    }
}
