using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Achievements.Queries
{
    public sealed class ListUserAchievementsQuery : IRequest<IList<ListUserAchievementsViewModel>>
    {
        #region Query
        public Guid Id { get; }
        public bool Completed { get; set; }

        public ListUserAchievementsQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserAchievementsQuery, IList<ListUserAchievementsViewModel>>
        {
            private readonly IAchievementRepository AchievementRepository;
            private readonly IMapper mapper;

            public QueryHandler(IAchievementRepository AchievementRepository, IMapper mapper)
            {
                this.AchievementRepository = AchievementRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserAchievementsViewModel>> Handle(ListUserAchievementsQuery request, CancellationToken cancellationToken)
            {
                var achievements = await AchievementRepository.ListByUserIdAsync(request.Id, cancellationToken);
                return mapper.Map<IEnumerable<ListUserAchievementsViewModel>>(achievements).ToList();
            }
        }
        #endregion
    }
}
