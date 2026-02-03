using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListAlumniGraduatedProgramQuery : IRequest<IList<ListAlumniGraduatedProgramViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListAlumniGraduatedProgramQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListAlumniGraduatedProgramQuery, IList<ListAlumniGraduatedProgramViewModel>>
        {
            private readonly IProgramRepository programRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramRepository programRepository, IMapper mapper)
            {
                this.programRepository = programRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListAlumniGraduatedProgramViewModel>> Handle(ListAlumniGraduatedProgramQuery request, CancellationToken cancellationToken)
            {
                var cohorts = await programRepository.ListAlumniGraduatedProgramAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListAlumniGraduatedProgramViewModel>>(cohorts).ToList();
            }
        }
        #endregion
    }
}
