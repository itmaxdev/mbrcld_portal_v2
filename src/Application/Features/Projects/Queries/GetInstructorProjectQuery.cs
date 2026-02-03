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
    public sealed class GetInstructorProjectQuery : IRequest<IList<GetInstructorProjectViewModel>>
    {
        #region Query
        public Guid ModuleId { get; }
        public Guid UserId { get; }

        public GetInstructorProjectQuery(Guid moduleId, Guid userId)
        {
            ModuleId = moduleId;
            UserId = userId;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetInstructorProjectQuery, IList<GetInstructorProjectViewModel>>
        {
            private readonly IProjectRepository projectRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectRepository projectRepository, IMapper mapper)
            {
                this.projectRepository = projectRepository;
                this.mapper = mapper;
            }

            public async Task<IList<GetInstructorProjectViewModel>> Handle(GetInstructorProjectQuery request, CancellationToken cancellationToken)
            {
                var projects = await projectRepository.GetProjectAsync(request.ModuleId, request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<GetInstructorProjectViewModel>>(projects).ToList();
            }
        }
        #endregion
    }
}
