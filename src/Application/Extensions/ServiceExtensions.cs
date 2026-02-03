using AutoMapper;
using FluentValidation;
using Mbrcld.Application.Behaviors;
using Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mbrcld.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var applicationAssembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);
            services.AddAutoMapper(applicationAssembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //services.AddTransient<IUserProfileSectionCompletionChecker, AchievementsSectionCompletionChecker>();
            services.AddTransient<IUserProfileSectionCompletionChecker, ContactDetailsSectionCompletionChecker>();
            services.AddTransient<IUserProfileSectionCompletionChecker, EducationQualificationsSectionCompletionChecker>();
            services.AddTransient<IUserProfileSectionCompletionChecker, GeneralInformationSectionCompletionChecker>();
            services.AddTransient<IUserProfileSectionCompletionChecker, IdentitySectionCompletionChecker>();
            //services.AddTransient<IUserProfileSectionCompletionChecker, InterestsSectionCompletionChecker>();
            //services.AddTransient<IUserProfileSectionCompletionChecker, LanguageSkillsSectionCompletionChecker>();
            services.AddTransient<IUserProfileSectionCompletionChecker, MembershipsSectionCompletionChecker>();
            services.AddTransient<IUserProfileSectionCompletionChecker, ProfessionalExperiencesSectionCompletionChecker>();
            //services.AddTransient<IUserProfileSectionCompletionChecker, ReferencesSectionCompletionChecker>();
            services.AddTransient<IUserProfileSectionCompletionChecker, TrainingCoursesSectionCompletionChecker>();
        }
    }
}
