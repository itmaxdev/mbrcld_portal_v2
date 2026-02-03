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
    public sealed class ListCommentsByPostIdQuery : IRequest<IList<ListCommentsByPostIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListCommentsByPostIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListCommentsByPostIdQuery, IList<ListCommentsByPostIdViewModel>>
        {
            private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(IPanHistoryRepository panHistoryRepository, IMapper mapper)
            {
                this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListCommentsByPostIdViewModel>> Handle(ListCommentsByPostIdQuery request, CancellationToken cancellationToken)
            {
                var comments = await panHistoryRepository.ListPanHistoriesCommentsByPostsAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<IEnumerable<ListCommentsByPostIdViewModel>>(comments).ToList();
            }
        }
        #endregion
    }
}
