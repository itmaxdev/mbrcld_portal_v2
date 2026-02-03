using AutoMapper;
using IdentityServer4.Stores;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Configuration;
using Mbrcld.Infrastructure.Identity.Services;
using Mbrcld.Infrastructure.Identity.Stores;
using Mbrcld.Infrastructure.Persistence;
using Mbrcld.Infrastructure.Persistence.Repositories;
using Mbrcld.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace Mbrcld.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var infrastructureAssembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(infrastructureAssembly);
            services.AddMemoryCache();

            var mscrmSettings = new MscrmSettings();
            configuration.GetSection("Mscrm").Bind(mscrmSettings);

            var mscrmWebApiUrl = new Uri(mscrmSettings.Url);
            var mscrmCredentials = new NetworkCredential(
                userName: mscrmSettings.Username,
                password: mscrmSettings.Password,
                domain: mscrmSettings.Domain
                );

            var credentialCache = new CredentialCache()
            {
                { mscrmWebApiUrl, "NTLM", mscrmCredentials }
            };

            services.AddHttpClient("MscrmODataWebApi", (client) =>
            {
                client.BaseAddress = mscrmWebApiUrl;
                client.Timeout = TimeSpan.FromMinutes(1);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("OData-Version", "4.0");
                client.DefaultRequestHeaders.TryAddWithoutValidation("OData-MaxVersion", "4.0");
            })
                .ConfigurePrimaryHttpMessageHandler((handler) =>
                {
                    return new HttpClientHandler()
                    {
                        AllowAutoRedirect = false,
                        Credentials = credentialCache
                    };
                });

            services.AddHttpClient<ISimpleWebApiClient, SimpleWebApiClient>("MscrmODataWebApi");
            services.AddHttpClient<IEmailService, EmailService>("MscrmODataWebApi");

            #region Identity stores
            services.AddScoped<IUserStore<User>, UserStore>();
            services.AddScoped<IRoleStore<Role>, RoleStore>();
            services.AddHttpClient<IPersistedGrantStore, PersistedGrantStore>("MscrmODataWebApi");
            #endregion

            #region Services
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserDocumentService, UserDocumentService>();
            services.AddTransient<IUserProfilePictureService, UserProfilePictureService>();
            services.AddTransient<IAttachedPictureService, AttachedPictureService>();
            services.AddTransient<IUploadFileService, UploadFileService>();
            #endregion

            #region Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICountryRepository, CachedCountryRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IScholarshipRepository, ScholarshipRepository>();
            services.AddTransient<IScholarshipRegistrationRepository, ScholarshipRegistrationRepository>();
            services.AddTransient<IEventQuestionRepository, EventQuestionRepository>();
            services.AddTransient<IProgramRepository, ProgramRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IProjectIdeaRepository, ProjectIdeaRepository>();
            services.AddTransient<ISpecialProjectRepository, SpecialProjectRepository>();
            services.AddTransient<IModuleRepository, ModuleRepository>();
            services.AddTransient<IMaterialRepository, MaterialRepository>();
            services.AddTransient<ISectionRepository, SectionRepository>();
            services.AddTransient<IContentRepository, ContentRepository>();
            services.AddTransient<INewsFeedRepository, NewsFeedRepository>();
            services.AddTransient<ICalendarRepository, CalendarRepository>();
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IPanHistoryRepository, PanHistoryRepository>();
            services.AddTransient<IEventRegistrantRepository, EventRegistrantRepository>();
            services.AddTransient<IProfessionalExperienceRepository, ProfessionalExperienceRepository>();
            services.AddTransient<IEducationQualificationRepository, EducationQualificationRepository>();
            services.AddTransient<IReferenceRepository, ReferenceRepository>();
            services.AddTransient<ITrainingCourseRepository, TrainingCourseRepository>();
            services.AddTransient<IMembershipRepository, MembershipRepository>();
            services.AddTransient<ILanguageSkillRepository, LanguageSkillRepository>();
            services.AddTransient<IInterestRepository, InterestRepository>();
            services.AddTransient<IAchievementRepository, AchievementRepository>();
            services.AddTransient<IEnrollmentRepository, EnrollmentRepository>();
            services.AddTransient<IProgramQuestionRepository, ProgramQuestionRepository>();
            services.AddTransient<IProgramAnswerRepository, ProgramAnswerRepository>();
            services.AddTransient<IMetadataRepository, MetadataRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<ISurveyRepository, SurveyRepository>();
            services.AddTransient<ITopicRepository, TopicRepository>();
            services.AddTransient<ISpecialProjectTopicRepository, SpecialProjectTopicRepository>();
            services.AddTransient<IUniversityRepository, UniversityRepository>();
            services.AddTransient<IUniversityTeamMemberRepository, UniversityTeamMemberRepository>();
            services.AddTransient<IEliteClubRepository, EliteClubRepository>();
            services.AddTransient<IEliteClubMemberRepository, EliteClubMemberRepository>();
            services.AddTransient<IEliteMembershipAttendanceRepository, EliteMembershipAttendanceRepository>();
            services.AddTransient<IMentorRepository, MentorRepository>();
            services.AddTransient<IEliteMentorSessionRepository, EliteMentorSessionRepository>();
            services.AddTransient<IEventAnswersRepository, EventAnswersRepository>();

            #endregion
        }
    }
}
