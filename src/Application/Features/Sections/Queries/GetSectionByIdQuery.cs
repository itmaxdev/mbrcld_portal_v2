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
    public sealed class GetSectionByIdQuery : IRequest<GetSectionByIdViewModel>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public GetSectionByIdQuery(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetSectionByIdQuery, GetSectionByIdViewModel>
        {
            private readonly ISectionRepository sectionRepository;
            private readonly IMapper mapper;

            public QueryHandler(ISectionRepository sectionRepository, IMapper mapper)
            {
                this.sectionRepository = sectionRepository;
                this.mapper = mapper;
            }

            public async Task<GetSectionByIdViewModel> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
            {
                var section = await sectionRepository.GetSectionByIdAsync(request.Id, request.UserId, cancellationToken);
                return mapper.Map<GetSectionByIdViewModel>(section);
            }
        }
        #endregion
    }
}
