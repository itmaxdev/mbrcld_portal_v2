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
    public sealed class ListModulesByEliteClubIdQuery : IRequest<IList<ListModulesByEliteClubIdViewModel>>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public ListModulesByEliteClubIdQuery(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListModulesByEliteClubIdQuery, IList<ListModulesByEliteClubIdViewModel>>
        {
            private readonly IModuleRepository moduleRepository;
            private readonly IMapper mapper;

            public QueryHandler(IModuleRepository moduleRepository, IMapper mapper)
            {
                this.moduleRepository = moduleRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListModulesByEliteClubIdViewModel>> Handle(ListModulesByEliteClubIdQuery request, CancellationToken cancellationToken)
            {
                var modules = await moduleRepository.ListModulesByEliteClubIdAsync(request.Id, request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListModulesByEliteClubIdViewModel>>(modules).ToList();
            }
        }
        #endregion
    }
}
