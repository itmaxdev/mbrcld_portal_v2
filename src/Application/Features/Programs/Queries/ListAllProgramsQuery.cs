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
    public sealed class ListAllProgramsQuery : IRequest<IList<ListAllProgramsViewModel>>
    {
        #region Query
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListAllProgramsQuery, IList<ListAllProgramsViewModel>>
        {
            private readonly IProgramRepository programRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramRepository programRepository, IMapper mapper)
            {
                this.programRepository = programRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListAllProgramsViewModel>> Handle(ListAllProgramsQuery request, CancellationToken cancellationToken)
            {
                var programs = await programRepository.ListAllProgramsAsync(cancellationToken);
                return mapper.Map<IEnumerable<ListAllProgramsViewModel>>(programs).ToList();
            }
        }
        #endregion
    }
}
