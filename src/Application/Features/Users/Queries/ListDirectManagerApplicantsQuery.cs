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
    public sealed class ListDirectManagerApplicantsQuery : IRequest<IList<ListDirectManagerApplicantsViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListDirectManagerApplicantsQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListDirectManagerApplicantsQuery, IList<ListDirectManagerApplicantsViewModel>>
        {
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                this.userRepository = userRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListDirectManagerApplicantsViewModel>> Handle(ListDirectManagerApplicantsQuery request, CancellationToken cancellationToken)
            {
                var applicants = await userRepository.ListDirectManagerApplicantsAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListDirectManagerApplicantsViewModel>>(applicants).ToList();
            }
        }
        #endregion
    }
}
