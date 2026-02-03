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
    public sealed class ListModuleApplicantsQuery : IRequest<IList<ListModuleApplicantsViewModel>>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public ListModuleApplicantsQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListModuleApplicantsQuery, IList<ListModuleApplicantsViewModel>>
        {
            private readonly IModuleRepository moduleRepository;
            private readonly IMapper mapper;

            public QueryHandler(IModuleRepository moduleRepository, IMapper mapper)
            {
                this.moduleRepository = moduleRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListModuleApplicantsViewModel>> Handle(ListModuleApplicantsQuery request, CancellationToken cancellationToken)
            {
                var moduleApplicants = await moduleRepository.ListModuleApplicantsAsync(request.Id,cancellationToken);

                return mapper.Map<IEnumerable<ListModuleApplicantsViewModel>>(moduleApplicants).ToList();
            }
        }
        #endregion
    }
}
