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
    public sealed class ListTopicsByProgramIdQuery : IRequest<IList<ListTopicsByProgramIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListTopicsByProgramIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListTopicsByProgramIdQuery, IList<ListTopicsByProgramIdViewModel>>
        {
            private readonly ITopicRepository topicRepository;
            private readonly IMapper mapper;

            public QueryHandler(ITopicRepository topicRepository, IMapper mapper)
            {
                this.topicRepository = topicRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListTopicsByProgramIdViewModel>> Handle(ListTopicsByProgramIdQuery request, CancellationToken cancellationToken)
            {
                var topics = await topicRepository.ListTopicsByProgramIdAsync(request.Id, cancellationToken);
                return mapper.Map<IEnumerable<ListTopicsByProgramIdViewModel>>(topics).ToList();
            }
        }
        #endregion
    }
}
