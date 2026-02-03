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
    public sealed class ListCommentsByArticleIdQuery : IRequest<IList<ListCommentsByArticleIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListCommentsByArticleIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListCommentsByArticleIdQuery, IList<ListCommentsByArticleIdViewModel>>
        {
            private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(IPanHistoryRepository panHistoryRepository, IMapper mapper)
            {
                this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListCommentsByArticleIdViewModel>> Handle(ListCommentsByArticleIdQuery request, CancellationToken cancellationToken)
            {
                var comments = await panHistoryRepository.ListPanHistoriesCommentsByArticlesAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<IEnumerable<ListCommentsByArticleIdViewModel>>(comments).ToList();
            }
        }
        #endregion
    }
}
