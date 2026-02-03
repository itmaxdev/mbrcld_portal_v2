using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries
{
    public sealed class GetUserLearningPreferencesQuery : IRequest<Maybe<GetUserLearningPreferencesViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public GetUserLearningPreferencesQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        public sealed class QueryHandler : IRequestHandler<GetUserLearningPreferencesQuery, Maybe<GetUserLearningPreferencesViewModel>>
        {
            private readonly IUserRepository userRepository;

            public QueryHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }

            public async Task<Maybe<GetUserLearningPreferencesViewModel>> Handle(GetUserLearningPreferencesQuery request, CancellationToken cancellationToken)
            {
                var user = await userRepository.GetByIdAsync(request.Id);
                if (user.HasNoValue)
                {
                    return Maybe<GetUserLearningPreferencesViewModel>.None;
                }

                return new GetUserLearningPreferencesViewModel(user.Value.LearningPreferences);
            }
        }
        #endregion
    }
}
