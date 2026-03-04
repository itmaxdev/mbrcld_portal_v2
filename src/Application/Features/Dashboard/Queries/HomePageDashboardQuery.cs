using AutoMapper;
using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Dashboard.Queries
{
    public sealed class HomePageDashboardQuery : IRequest<List<DashboardViewModel>>
    {
        #region Query
         public Guid UserId { get; }

        public HomePageDashboardQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<HomePageDashboardQuery, List<DashboardViewModel>>
        {
            private readonly IProgramRepository programRepository;
            private readonly IArticleRepository articleRepository;
            private readonly IEventRepository eventRepository;
            private readonly IUserRepository userRepository;

            private readonly IMapper mapper;

            public QueryHandler(
                IUserRepository userRepository,
                IProgramRepository programRepository,
                IArticleRepository articleRepository,
                IEventRepository eventRepository,
                IMapper mapper)
            {
                this.userRepository = userRepository;
                this.programRepository = programRepository;
                this.articleRepository = articleRepository;
                this.eventRepository = eventRepository;
                this.mapper = mapper;
            }

            public async Task<List<DashboardViewModel>> Handle(HomePageDashboardQuery request, CancellationToken cancellationToken)
            {
                List<DashboardViewModel> Dashboard = new List<DashboardViewModel> { };
                DashboardViewModel Programs = new DashboardViewModel{ };                
                DashboardViewModel Events = new DashboardViewModel { };
                DashboardViewModel Articles = new DashboardViewModel { };
                var user = await userRepository.GetByIdAsync(request.UserId);

                //KPI's
                var events = await eventRepository.GetEventsAsync(request.UserId, cancellationToken);

                var articles = await articleRepository.ListArticlesAsync( cancellationToken);
                
                //var activePrograms = await programRepository.ListActiveProgramsAsync(cancellationToken);
                var programs = await programRepository.ListAllProgramsAsync(cancellationToken);
                

                Events.Name = "Total Events";
                Events.Name_AR = "جميع الفعاليات";
                Events.Value = events.Count();

                Programs.Name = "Total Programs";
                Programs.Name_AR = "جميع البرامج";
                Programs.Value = programs.Count();

                Articles.Name = "Total Articles";
                Articles.Name_AR = "جميع المقالات";
                Articles.Value = articles.Count();

                Dashboard.Add(Events);
                Dashboard.Add(Programs);
                Dashboard.Add(Articles);

                return mapper.Map<List<DashboardViewModel>>(Dashboard);
            }
        }
        #endregion
    }
}
