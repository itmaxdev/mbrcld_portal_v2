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
    public sealed class ListContentsBySectionIdQuery : IRequest<IList<ListContentsBySectionIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListContentsBySectionIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListContentsBySectionIdQuery, IList<ListContentsBySectionIdViewModel>>
        {
            private readonly IContentRepository contentRepository;
            private readonly IMapper mapper;

            public QueryHandler(IContentRepository contentRepository, IMapper mapper)
            {
                this.contentRepository = contentRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListContentsBySectionIdViewModel>> Handle(ListContentsBySectionIdQuery request, CancellationToken cancellationToken)
            {
                var contents = await contentRepository.ListContentsBySectionIdAsync(request.Id, cancellationToken);

                return mapper.Map<IEnumerable<ListContentsBySectionIdViewModel>>(contents).ToList();
            }
        }
        #endregion
    }
}
