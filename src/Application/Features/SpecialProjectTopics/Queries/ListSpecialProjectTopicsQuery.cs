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
    public sealed class ListSpecialProjectTopicsQuery : IRequest<IList<ListSpecialProjectTopicsViewModel>>
    {
        #region Query
        #endregion
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListSpecialProjectTopicsQuery, IList<ListSpecialProjectTopicsViewModel>>
        {
            private readonly ISpecialProjectTopicRepository specialProjectTopicRepository;
            private readonly IMapper mapper;

            public QueryHandler(ISpecialProjectTopicRepository specialProjectTopicRepository, IMapper mapper)
            {
                this.specialProjectTopicRepository = specialProjectTopicRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListSpecialProjectTopicsViewModel>> Handle(ListSpecialProjectTopicsQuery request, CancellationToken cancellationToken)
            {
                var topics = await specialProjectTopicRepository.ListSpecialProjectTopicsAsync(cancellationToken);
                return mapper.Map<IEnumerable<ListSpecialProjectTopicsViewModel>>(topics).ToList();
            }
        }
        #endregion
    }
}
