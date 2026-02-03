using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries
{
    public sealed class GetUserByPhoneNumber : IRequest<Maybe<GetUserByPhoneNumberViewModel>>
    {
        #region Query
        public string PhoneNumber { get; }

        public GetUserByPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetUserByPhoneNumber, Maybe<GetUserByPhoneNumberViewModel>>
        {
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                this.userRepository = userRepository;
                this.mapper = mapper;
            }

            public async Task<Maybe<GetUserByPhoneNumberViewModel>> Handle(GetUserByPhoneNumber request, CancellationToken cancellationToken)
            {
                var user = await userRepository.GetByPhoneNumberAsync(request.PhoneNumber);
                var viewModel = mapper.Map<GetUserByPhoneNumberViewModel>(user.ValueOrDefault);
                return viewModel;
            }
        }
        #endregion
    }
}
