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
    public sealed class ListAlumniAvailableProgramQuery : IRequest<IList<ListAlumniAvailableProgramViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListAlumniAvailableProgramQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListAlumniAvailableProgramQuery, IList<ListAlumniAvailableProgramViewModel>>
        {
            private readonly IProgramRepository programRepository;
            private readonly IMapper mapper;
            private readonly IConfiguration configuration;

            public QueryHandler(IProgramRepository programRepository, IConfiguration configuration, IMapper mapper)
            {
                this.programRepository = programRepository;
                this.configuration = configuration;
                this.mapper = mapper;
            }

            public async Task<IList<ListAlumniAvailableProgramViewModel>> Handle(ListAlumniAvailableProgramQuery request, CancellationToken cancellationToken)
            {
                var year = configuration.GetSection("Mscrm").GetChildren().Where(x => x.Path == "Mscrm:Period_Year").FirstOrDefault().Value;
                var programs = await programRepository.ListAlumniAvailableProgramAsync(request.UserId, year, cancellationToken);
                return mapper.Map<IEnumerable<ListAlumniAvailableProgramViewModel>>(programs).ToList();
            }
        }
        #endregion
    }
}
