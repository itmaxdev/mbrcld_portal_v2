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
    public sealed class GetSpecialProjectByIdQuery : IRequest<GetSpecialProjectByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetSpecialProjectByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetSpecialProjectByIdQuery, GetSpecialProjectByIdViewModel>
        {
            private readonly ISpecialProjectRepository specialProjectRepository;
            private readonly IMapper mapper;

            public QueryHandler(ISpecialProjectRepository specialProjectRepository, IMapper mapper)
            {
                this.specialProjectRepository = specialProjectRepository;
                this.mapper = mapper;
            }

            public async Task<GetSpecialProjectByIdViewModel> Handle(GetSpecialProjectByIdQuery request, CancellationToken cancellationToken)
            {
                var specialProject = await specialProjectRepository.GetSpecialProjectByIdAsync(request.Id, cancellationToken);
                return mapper.Map<GetSpecialProjectByIdViewModel>(specialProject.ValueOrDefault);
            }
        }
        #endregion
    }
}
