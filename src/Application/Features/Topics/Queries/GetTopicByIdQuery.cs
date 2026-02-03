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
    public sealed class GetTopicByIdQuery : IRequest<GetTopicByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetTopicByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetTopicByIdQuery, GetTopicByIdViewModel>
        {
            private readonly ITopicRepository topicRepository;
            private readonly IMapper mapper;

            public QueryHandler(ITopicRepository topictRepository, IMapper mapper)
            {
                this.topicRepository = topictRepository;
                this.mapper = mapper;
            }

            public async Task<GetTopicByIdViewModel> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
            {
                var events = await topicRepository.GetTopicByIdAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<GetTopicByIdViewModel>(events.ValueOrDefault);
            }
        }
        #endregion
    }
}
