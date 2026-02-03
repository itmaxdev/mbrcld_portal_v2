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
    public sealed class ListSectionsByCohortMaterialIdQuery : IRequest<IList<ListSectionsByCohortMaterialIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListSectionsByCohortMaterialIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListSectionsByCohortMaterialIdQuery, IList<ListSectionsByCohortMaterialIdViewModel>>
        {
            private readonly ISectionRepository sectionRepository;
            private readonly IMapper mapper;

            public QueryHandler(ISectionRepository sectionRepository, IMapper mapper)
            {
                this.sectionRepository = sectionRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListSectionsByCohortMaterialIdViewModel>> Handle(ListSectionsByCohortMaterialIdQuery request, CancellationToken cancellationToken)
            {
                var sections = await sectionRepository.ListSectionsByCohortMaterialIdAsync(request.Id, cancellationToken);
                return mapper.Map<IEnumerable<ListSectionsByCohortMaterialIdViewModel>>(sections).ToList();
            }
        }
        #endregion
    }
}
