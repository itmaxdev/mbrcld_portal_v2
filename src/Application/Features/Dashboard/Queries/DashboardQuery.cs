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
    public sealed class DashboardQuery : IRequest<List<DashboardViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public DashboardQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<DashboardQuery, List<DashboardViewModel>>
        {
            private readonly IUserRepository userRepository;
            private readonly IProgramRepository programRepository;
            private readonly IArticleRepository articleRepository;
            private readonly IModuleRepository moduleRepository;
            private readonly ISectionRepository sectionRepository;
            private readonly IProjectRepository projectRepository;
            private readonly IProjectIdeaRepository projectIdeaRepository;
            private readonly IEventRegistrantRepository eventRegistrantRepository;
            private readonly ISurveyRepository surveyRepository;
            private readonly IMapper mapper;

            public QueryHandler(
                IUserRepository userRepository,
                IProgramRepository programRepository,
                IArticleRepository articleRepository,
                IModuleRepository moduleRepository,
                ISectionRepository sectionRepository,
                IProjectRepository projectRepository,
                IProjectIdeaRepository projectIdeaRepository,
                IEventRegistrantRepository eventRegistrantRepository,
                ISurveyRepository surveyRepository,
                IMapper mapper)
            {
                this.userRepository = userRepository;
                this.programRepository = programRepository;
                this.articleRepository = articleRepository;
                this.moduleRepository = moduleRepository;
                this.sectionRepository = sectionRepository;
                this.projectRepository = projectRepository;
                this.projectIdeaRepository = projectIdeaRepository;
                this.eventRegistrantRepository = eventRegistrantRepository;
                this.surveyRepository = surveyRepository;
                this.mapper = mapper;
            }

            public async Task<List<DashboardViewModel>> Handle(DashboardQuery request, CancellationToken cancellationToken)
            {
                List<DashboardViewModel> Dashboard = new List<DashboardViewModel> { };
                //List<DashboardViewModel.KPI> KPIs = new List<DashboardViewModel.KPI> { };
                //List<DashboardViewModel.KPI> Programs = new List<DashboardViewModel.KPI> { };
                DashboardViewModel Projects = new DashboardViewModel { };
                DashboardViewModel Modules = new DashboardViewModel { };
                DashboardViewModel Sections = new DashboardViewModel { };
                DashboardViewModel RegisteredEvents = new DashboardViewModel { };
                DashboardViewModel Articles = new DashboardViewModel { };
                DashboardViewModel Surveys = new DashboardViewModel { };
                DashboardViewModel ProjectIdeas = new DashboardViewModel { };
                //List<DashboardViewModel.DashboardModule> DashboardModules = new List<DashboardViewModel.DashboardModule> { };
                //List<DashboardViewModel.DashboardProgram> DashboardPrograms = new List<DashboardViewModel.DashboardProgram> { };

                //KPI's
                var userModule = await moduleRepository.ListModulesByUserIdAsync(request.UserId, cancellationToken);
                var InProgressProject = await projectRepository.ListProjectsAsync(request.UserId, cancellationToken);
                var CompletedProject = await projectRepository.ListCompletedProjectsAsync(request.UserId, cancellationToken);
                var UnreadSections = await sectionRepository.ListUnreadSectionsByUserIdAsync(request.UserId, cancellationToken);
                var ReviewSections = await sectionRepository.ListReviewSectionsByUserIdAsync(request.UserId, cancellationToken);
                var DoneSections = await sectionRepository.ListDoneSectionsByUserIdAsync(request.UserId, cancellationToken);
                var EventRegistrant = await eventRegistrantRepository.ListEventRegistrantByUserIdAsync(request.UserId, cancellationToken);
                var UserCompletedSurveys = await surveyRepository.ListUserCompletedSurveysAsync(request.UserId, cancellationToken);

                var user = await userRepository.GetByIdAsync(request.UserId);

                int publishedProjectIdeas = 0;
                int inProgressProjectIdeas = 0;
                int publishedArticles = 0;
                int inProgressArticles = 0;


                if (user.Value.Role == 3)//alumni
                {
                    var alumniProjectIdeas = await projectIdeaRepository.ListUserProjectIdeasAsync(request.UserId, cancellationToken);
                    foreach (var alumniProjectIdea in alumniProjectIdeas)
                    {
                        if (alumniProjectIdea.ProjectIdeaStatus == 3)// Published
                        {
                            publishedProjectIdeas++;
                        }
                        else
                        {
                            inProgressProjectIdeas++;
                        }
                    }
                }

                var articles = await articleRepository.ListUserArticlesAsync(request.UserId, cancellationToken);
                foreach (var article in articles)
                {
                    if (article.ArticleStatus == 3)// Published
                    {
                        publishedArticles++;
                    }
                    else
                    {
                        inProgressArticles++;
                    }
                }

                //Programs
                //var userProgram = await programRepository.ListUserProgramsAsync(request.UserId, cancellationToken);
                //foreach (var p in userProgram)
                //{
                //    DashboardPrograms.Add(new DashboardViewModel.DashboardProgram
                //    {
                //        Name = p.Name,
                //        Description = p.Description,
                //        Value = p.Completed,
                //        ID = p.Id,
                //        URL = "/attached-pictures/" + p.Id
                //    });
                //}

                ////Modules
                //var userModules = await moduleRepository.ListUserModulesAsync(request.UserId, cancellationToken);
                //foreach (var m in userModules)
                //{
                //    DashboardModules.Add(new DashboardViewModel.DashboardModule
                //    {
                //        Name = m.Name,
                //        Description = m.Description,
                //        Value = m.Completed.Value,
                //        ID = m.Id,
                //        ProgramId = m.ProgramId
                //    });
                //}

                Modules.Name = "Module";
                Modules.Name_AR = "المساقات";
                Modules.Value = userModule;

                Projects.Name = "Projects";
                Projects.Name_AR = "المشاريع";
                Projects.Value = InProgressProject.Count + CompletedProject.Count;
                Projects.Details = new List<DashboardViewModel.KPI> { };
                Projects.Details.Add(new DashboardViewModel.KPI
                {
                    Name = "In Progress",
                    Name_AR = "قيد العمل",
                    Value = InProgressProject.Count,
                    Order = 1
                });
                Projects.Details.Add(new DashboardViewModel.KPI
                {
                    Name = "Completed",
                    Name_AR = "مكتمل",
                    Value = CompletedProject.Count,
                    Order = 2
                });

                Sections.Name = "Sections";
                Sections.Name_AR = "الأقسام";
                Sections.Value = ReviewSections + UnreadSections +DoneSections;
                Sections.Details = new List<DashboardViewModel.KPI> { };
                Sections.Details.Add(new DashboardViewModel.KPI
                {
                    Name = "Unread",
                    Name_AR = "غير مقروء",
                    Value = UnreadSections,
                    Order = 0
                });
                Sections.Details.Add(new DashboardViewModel.KPI
                {
                    Name = "Reviewed",
                    Name_AR = "قيد المراجعة",
                    Value = ReviewSections,
                    Order = 1
                });
                Sections.Details.Add(new DashboardViewModel.KPI
                {
                    Name = "Completed",
                    Name_AR = "مكتمل",
                    Value = DoneSections,
                    Order = 2
                });

                Surveys.Name = "Surveys";
                Surveys.Name_AR = "التقييمات";
                Surveys.Value = UserCompletedSurveys.Count;

                RegisteredEvents.Name = "Registered Events";
                RegisteredEvents.Name_AR = "الفعاليات";
                RegisteredEvents.Value = EventRegistrant;

                Articles.Name = "Articles";
                Articles.Name_AR = "المقالات";
                Articles.Value = publishedArticles + inProgressArticles;
                Articles.Details = new List<DashboardViewModel.KPI> { };
                Articles.Details.Add(new DashboardViewModel.KPI
                {
                    Name = "Published",
                    Name_AR = "نشر",
                    Value = publishedArticles,
                    Order = 0
                });
                Articles.Details.Add(new DashboardViewModel.KPI
                {
                    Name = "In Progress",
                    Name_AR = "قيد العمل",
                    Value = inProgressArticles,
                    Order = 1
                });

                if (user.Value.Role == 3)//alumni
                {
                    ProjectIdeas.Name = "Idea Hub";
                    ProjectIdeas.Name_AR = "مختبر الأفكار";
                    ProjectIdeas.Value = publishedProjectIdeas + inProgressProjectIdeas;
                    ProjectIdeas.Details = new List<DashboardViewModel.KPI> { };
                    ProjectIdeas.Details.Add(new DashboardViewModel.KPI
                    {
                        Name = "Published",
                        Name_AR = "نشر",
                        Value = publishedProjectIdeas,
                        Order = 0
                    });

                    ProjectIdeas.Details.Add(new DashboardViewModel.KPI
                    {
                        Name = "In Progress",
                        Name_AR = "قيد العمل",
                        Value = inProgressProjectIdeas,
                        Order = 1
                    });
                }

                Dashboard.Add(Modules);
                Dashboard.Add(Sections);
                Dashboard.Add(Projects);
                Dashboard.Add(Surveys);
                Dashboard.Add(RegisteredEvents);
                Dashboard.Add(Articles);

                if (user.Value.Role == 3)//alumni
                {
                    Dashboard.Add(ProjectIdeas);
                }

                //Dashboard.Programs = Programs;
                //Dashboard.Projects = Projects;
                //Dashboard.RegisteredEvents = RegisteredEvents;
                //Dashboard.Articles = Articles;
                //Dashboard.Modules = Modules;
                //Dashboard.Sections = Sections;
                //Dashboard.Surveys = Surveys;
                //Dashboard.ProjectIdeas = ProjectIdeas;
                //Dashboard.DashboardModules = DashboardModules;
                //Dashboard.DashboardPrograms = DashboardPrograms;
                return mapper.Map<List<DashboardViewModel>>(Dashboard);
            }
        }
        #endregion
    }
}
