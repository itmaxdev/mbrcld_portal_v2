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
    public sealed class ListAlumniUsersForChatQuery : IRequest<IList<ListAlumniUsersForChatViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListAlumniUsersForChatQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListAlumniUsersForChatQuery, IList<ListAlumniUsersForChatViewModel>>
        {
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                this.userRepository = userRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListAlumniUsersForChatViewModel>> Handle(ListAlumniUsersForChatQuery request, CancellationToken cancellationToken)
            {
                var users = await userRepository.ListAlumniUsersForChatAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListAlumniUsersForChatViewModel>>(users).ToList();
            }
        }
        #endregion
    }
}
