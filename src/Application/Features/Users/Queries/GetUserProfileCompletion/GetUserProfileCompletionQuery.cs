using Mbrcld.Application.Exceptions;
using Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion
{
    public sealed class GetUserProfileCompletionQuery : IRequest<GetUserProfileCompletionViewModel>
    {
        public Guid UserId { get; }

        public GetUserProfileCompletionQuery(Guid userId)
        {
            this.UserId = userId;
        }

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetUserProfileCompletionQuery, GetUserProfileCompletionViewModel>
        {
            private readonly IUserRepository userRepository;
            private readonly IEnumerable<IUserProfileSectionCompletionChecker> sectionCompletionCheckers;

            public QueryHandler(IUserRepository userRepository, IEnumerable<IUserProfileSectionCompletionChecker> sectionCompletionCheckers)
            {
                this.userRepository = userRepository;
                this.sectionCompletionCheckers = sectionCompletionCheckers;
            }

            public async Task<GetUserProfileCompletionViewModel> Handle(GetUserProfileCompletionQuery request, CancellationToken cancellationToken)
            {
                var user = (User)await this.userRepository.GetByIdAsync(request.UserId, cancellationToken);

                var completion = new Dictionary<string, (int Weight, bool IsComplete)>();

                foreach (var checker in this.sectionCompletionCheckers)
                {
                    var weight = await checker.GetSectionWeightAsync();
                    var identifier = await checker.GetSectionIdentifierAsync();
                    var isComplete = await checker.IsSectionCompleteAsync(user);
                    completion[identifier] = (weight, isComplete);

                }

                var weightOfCompletedSections = completion.Values.Where(x => x.IsComplete).Sum(x => x.Weight);
                var totalWeight = completion.Values.Sum(x => x.Weight);

                var completionPercentage = (int)Math.Floor(((double)weightOfCompletedSections / totalWeight) * 100.0);

                // Set minimum to 1%
                if (completionPercentage < 1)
                {
                    completionPercentage = 1;
                }

                return new GetUserProfileCompletionViewModel()
                {
                    CompletionPercentage = completionPercentage,
                    Sections = completion.Select(
                        x => new KeyValuePair<string, bool>(x.Key, x.Value.IsComplete)).ToList()
                };
            }
        }
        #endregion
    }
}
