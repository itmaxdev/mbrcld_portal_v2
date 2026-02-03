using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class PinnedPostsQuery : IRequest<PinnedPostsViewModel>
    {
        #region Query

        public PinnedPostsQuery()
        {
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<PinnedPostsQuery, PinnedPostsViewModel>
        {
            private readonly IPostRepository postRepository;
            private readonly IMapper mapper;

            public QueryHandler(IPostRepository postRepository, IMapper mapper)
            {
                this.postRepository = postRepository;
                this.mapper = mapper;
            }

            public async Task<PinnedPostsViewModel> Handle(PinnedPostsQuery request, CancellationToken cancellationToken)
            {
               
                var posts = await postRepository.PinnedPostsAsync(cancellationToken);

                return mapper.Map<PinnedPostsViewModel>(posts);

            }
        }
        #endregion
        
    }
}
