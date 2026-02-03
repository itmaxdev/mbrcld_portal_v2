using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetProjectByIdQuery : IRequest<GetProjectByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetProjectByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetProjectByIdQuery, GetProjectByIdViewModel>
        {
            private readonly IProjectRepository projectRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectRepository projectRepository, IMapper mapper)
            {
                this.projectRepository = projectRepository;
                this.mapper = mapper;
            }

            public async Task<GetProjectByIdViewModel> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
            {
                var project = await projectRepository.GetProjectByIdAsync(request.Id, cancellationToken);
                return mapper.Map<GetProjectByIdViewModel>(project.Value);
            }
        }
        #endregion
    }
}
