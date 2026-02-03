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
    public sealed class ListSectionsByMaterialIdQuery : IRequest<IList<ListSectionsByMaterialIdViewModel>>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public ListSectionsByMaterialIdQuery(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListSectionsByMaterialIdQuery, IList<ListSectionsByMaterialIdViewModel>>
        {
            private readonly ISectionRepository sectionRepository;
            private readonly IMapper mapper;

            public QueryHandler(ISectionRepository sectionRepository, IMapper mapper)
            {
                this.sectionRepository = sectionRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListSectionsByMaterialIdViewModel>> Handle(ListSectionsByMaterialIdQuery request, CancellationToken cancellationToken)
            {
                var sections = await sectionRepository.ListSectionsByMaterialIdAsync(request.Id, request.UserId, cancellationToken);

                return mapper.Map<IEnumerable<ListSectionsByMaterialIdViewModel>>(sections).ToList();
            }
        }
        #endregion
    }
}
