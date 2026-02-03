using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries
{
    public sealed class GetUserProfileQuery : IRequest<Maybe<GetUserProfileViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public GetUserProfileQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetUserProfileQuery, Maybe<GetUserProfileViewModel>>
        {
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                this.userRepository = userRepository;
                this.mapper = mapper;
            }

            public async Task<Maybe<GetUserProfileViewModel>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
            {
                var user = await userRepository.GetByIdAsync(request.Id);
                var viewModel = mapper.Map<GetUserProfileViewModel>(user.ValueOrDefault);
                return viewModel;
            }
        }
        #endregion
    }
}
