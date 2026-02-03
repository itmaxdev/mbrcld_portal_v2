using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.References.Queries
{
    public sealed class ListUserReferencesQuery : IRequest<IList<ListUserReferencesViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListUserReferencesQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserReferencesQuery, IList<ListUserReferencesViewModel>>
        {
            private readonly IReferenceRepository referenceRepository;
            private readonly IMapper mapper;

            public QueryHandler(IReferenceRepository referenceRepository, IMapper mapper)
            {
                this.referenceRepository = referenceRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserReferencesViewModel>> Handle(ListUserReferencesQuery request, CancellationToken cancellationToken)
            {
                var references = await referenceRepository.ListByUserIdAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<IList<ListUserReferencesViewModel>>(references);
            }
        }
        #endregion
    }
}
