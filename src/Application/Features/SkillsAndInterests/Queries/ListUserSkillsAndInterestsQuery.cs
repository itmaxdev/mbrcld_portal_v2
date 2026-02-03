using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.SkillsAndInterests.Queries
{
    public sealed class ListUserSkillsAndInterestsQuery : IRequest<IList<ListUserSkillsAndInterestsViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListUserSkillsAndInterestsQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserSkillsAndInterestsQuery, IList<ListUserSkillsAndInterestsViewModel>>
        {
            private readonly IInterestRepository interestRepository;
            private readonly IMapper mapper;

            public QueryHandler(IInterestRepository interestRepository, IMapper mapper)
            {
                this.interestRepository = interestRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserSkillsAndInterestsViewModel>> Handle(ListUserSkillsAndInterestsQuery request, CancellationToken cancellationToken)
            {
                var Skill_Interests = await interestRepository.ListByUserIdAsync(request.Id, cancellationToken);
                return mapper.Map<IEnumerable<ListUserSkillsAndInterestsViewModel>>(Skill_Interests).ToList();
            }
        }
        #endregion
    }
}
