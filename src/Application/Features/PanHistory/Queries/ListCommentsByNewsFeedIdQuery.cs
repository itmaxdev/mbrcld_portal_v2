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
    public sealed class ListCommentsByNewsFeedIdQuery : IRequest<IList<ListCommentsByNewsFeedIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListCommentsByNewsFeedIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListCommentsByNewsFeedIdQuery, IList<ListCommentsByNewsFeedIdViewModel>>
        {
            private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(IPanHistoryRepository panHistoryRepository, IMapper mapper)
            {
                this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListCommentsByNewsFeedIdViewModel>> Handle(ListCommentsByNewsFeedIdQuery request, CancellationToken cancellationToken)
            {
                var comments = await panHistoryRepository.ListPanHistoriesCommentsByNewsFeedsAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<IEnumerable<ListCommentsByNewsFeedIdViewModel>>(comments).ToList();
            }
        }
        #endregion
    }
}
