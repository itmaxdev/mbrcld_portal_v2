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
    public sealed class ListEliteMentorSessionsByUserIdQuery : IRequest<IList<ListEliteMentorSessionsViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListEliteMentorSessionsByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
        #endregion
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListEliteMentorSessionsByUserIdQuery, IList<ListEliteMentorSessionsViewModel>>
        {
            private readonly IEliteMentorSessionRepository eliteMentorSessionRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEliteMentorSessionRepository eliteMentorSessionRepository, IMapper mapper)
            {
                this.eliteMentorSessionRepository = eliteMentorSessionRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListEliteMentorSessionsViewModel>> Handle(ListEliteMentorSessionsByUserIdQuery request, CancellationToken cancellationToken)
            {
                var eliteMentorSessions = await eliteMentorSessionRepository.ListEliteMentorSessionByUserIdAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListEliteMentorSessionsViewModel>>(eliteMentorSessions).ToList();
            }
        }
        #endregion
    }
}
