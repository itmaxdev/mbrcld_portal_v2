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
    public sealed class ListActiveProgramsQuery : IRequest<IList<ListActiveProgramsViewModel>>
    {
        #region Query
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListActiveProgramsQuery, IList<ListActiveProgramsViewModel>>
        {
            private readonly IProgramRepository programRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramRepository programRepository, IMapper mapper)
            {
                this.programRepository = programRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListActiveProgramsViewModel>> Handle(ListActiveProgramsQuery request, CancellationToken cancellationToken)
            {
                var programs = await programRepository.ListActiveProgramsAsync(cancellationToken);
                return mapper.Map<IEnumerable<ListActiveProgramsViewModel>>(programs).ToList();
            }
        }
        #endregion
    }
}
