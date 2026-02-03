using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries
{
    public sealed class GetUserDisclaimerQuery : IRequest<Maybe<GetUserDisclaimerViewModel>>
    {
        #region Query
        public Guid Id { get; }
        public int DisclaimerType { get; }

        public GetUserDisclaimerQuery(Guid id, int disclaimerType)
        {
            Id = id;
            DisclaimerType = disclaimerType;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetUserDisclaimerQuery, Maybe<GetUserDisclaimerViewModel>>
        {
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                this.userRepository = userRepository;
                this.mapper = mapper;
            }

            public async Task<Maybe<GetUserDisclaimerViewModel>> Handle(GetUserDisclaimerQuery request, CancellationToken cancellationToken)
            {
                var user = await userRepository.GetByIdAsync(request.Id);
                var viewModel = mapper.Map<GetUserDisclaimerViewModel>(user.ValueOrDefault);

                if (request.DisclaimerType == 1)//ArticlesDisclaimer
                {
                    viewModel.Disclaimer = user.Value.ArticlesDisclaimer;
                }
                if (request.DisclaimerType == 2)//IdeaHubDisclaimer
                {
                    viewModel.Disclaimer = user.Value.IdeaHubDisclaimer;
                }
                if (request.DisclaimerType == 3)//SpecialProjectDisclaimer
                {
                    viewModel.Disclaimer = user.Value.SpecialProjectDisclaimer;
                }
                return viewModel;
            }
        }
        #endregion
    }
}
