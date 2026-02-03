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
    public sealed class GetPostByIdQuery : IRequest<GetPostByIdViewModel>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public GetPostByIdQuery(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetPostByIdQuery, GetPostByIdViewModel>
        {
            private readonly IPostRepository postRepository;
            private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(IPostRepository postRepository, IPanHistoryRepository panHistoryRepository, IMapper mapper)
            {
                this.postRepository = postRepository;
                this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<GetPostByIdViewModel> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
            {
                var post = await postRepository.GetPostByIdAsync(request.Id, cancellationToken);
                if (post.HasValue)
                {
                    var postlikes = await panHistoryRepository.ListPanHistoriesByPostsAsync(post.Value.Id).ConfigureAwait(false);
                    foreach (var likes in postlikes)
                    {
                        if (likes.UserId == request.UserId)
                        {
                            post.Value.Liked = true;
                            break;
                        }
                    }

                    if (postlikes.Count > 0)
                    {
                        post.Value.Likes = postlikes.Count;
                    }
                }
                return mapper.Map<GetPostByIdViewModel>(post.ValueOrDefault);
            }
        }
        #endregion
    }
}
