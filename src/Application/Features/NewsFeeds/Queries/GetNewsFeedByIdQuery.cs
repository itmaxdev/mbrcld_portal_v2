using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetNewsFeedByIdQuery : IRequest<GetNewsFeedByIdViewModel>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public GetNewsFeedByIdQuery(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetNewsFeedByIdQuery, GetNewsFeedByIdViewModel>
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

            public async Task<GetNewsFeedByIdViewModel> Handle(GetNewsFeedByIdQuery request, CancellationToken cancellationToken)
            {
                var newsfeed = await newsFeedRepository.GetNewsFeedByIdAsync(request.Id, cancellationToken);
                if (newsfeed.HasValue)
                {
                    var newsfeedlikes = await panHistoryRepository.ListPanHistoriesByNewsFeedsAsync(request.Id).ConfigureAwait(false);
                    foreach (var likes in newsfeedlikes)
                    {
                        if (likes.UserId == request.UserId)
                        {
                            newsfeed.Value.Liked = true;
                            break;
                        }
                    }

                    if (newsfeedlikes.Count > 0)
                    {
                        newsfeed.Value.Likes = newsfeedlikes.Count;
                    }
                }
                return mapper.Map<GetNewsFeedByIdViewModel>(newsfeed.ValueOrDefault);
            }
        }
        #endregion
    }
}
