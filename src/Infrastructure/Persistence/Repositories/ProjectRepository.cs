using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class ProjectRepository : IProjectRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public ProjectRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<IList<Project>> ListProjectsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProjects = await webApiClient.For<ODataProject>()
                .Filter(c => c.Applicant.ContactId == userId)
                .Filter(c => c.ProjectStatus != 5) // Not Completed
                .Filter(c => c.TeamLead == false)
                .OrderBy(x => x.StartDate)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Project>>(odataProjects);
        }

        public async Task<IList<Project>> ListCompletedProjectsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProjects = await webApiClient.For<ODataProject>()
                .Filter(c => c.Applicant.ContactId == userId)
                .Filter(c => c.ProjectStatus == 5) // Completed
                .OrderBy(x => x.StartDate)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Project>>(odataProjects);
        }
        public async Task<IList<Project>> ListProjectsByCohortIdAsync(Guid cohortId, Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProjects = await webApiClient.For<ODataProject>()
                .Filter(c => c.Cohort.Id == cohortId)
                .Filter(c => c.Applicant.ContactId == userId)
                .OrderBy(x => x.StartDate)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Project>>(odataProjects);
        }

        public async Task<IList<Project>> ListInstructorProjectsAsync(Guid userId, Guid moduleId, CancellationToken cancellationToken = default)
        {
            var odataProjects = await webApiClient.For<ODataProject>()
                .Filter(c => c.Instructor.ContactId == userId)
                .Filter(c => c.Module.Id == moduleId)
                .Filter(c => c.Template == false)
                .Filter(c => c.ProjectStatus == 4)  //Under Review
                .OrderBy(x => x.StartDate)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Project>>(odataProjects);
        }

        public async Task<IList<Project>> GetProjectAsync(Guid moduleId, Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProjects = await webApiClient.For<ODataProject>()
               .Filter(c => c.Instructor.ContactId == userId)
               .Filter(c => c.Module.Id == moduleId)
               .Filter(c => c.Template == true)
               .ProjectToModel()
               .FindEntriesAsync(cancellationToken)
               .ConfigureAwait(false);

            return this.mapper.Map<IList<Project>>(odataProjects);
        }

        public async Task<Result> CreateInstructorProjectAsync(Project project, CancellationToken cancellationToken = default)
        {
            var odataproject = this.mapper.Map<ODataProject>(project);
            odataproject.Applicant = null;
            odataproject.Topic = null;
            odataproject.MainProject = null;

            await webApiClient.For<ODataProject>()
                 .Set(odataproject)
                 .InsertEntryAsync(false, cancellationToken)
                 .ConfigureAwait(false);

            return Result.Success();
        }
        public async Task<Maybe<Project>> GetProjectByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataProject = await webApiClient.For<ODataProject>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Project>(odataProject);
        }
        public async Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
        {
            var odataproject = this.mapper.Map<ODataProject>(project);

            if (project.ApplicantId == null)
            {
                odataproject.Applicant = null;
            }
            if (project.InstructorId == null)
            {
                odataproject.Instructor = null;
            }
            if (project.ProgramId == null)
            {
                odataproject.Program = null;
            }
            else
            {
                var program = await webApiClient.For<ODataProgram>()
                .Key(project.ProgramId)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);
                var activeCohortId = program.ActiveCohort.Id;
                var cohort = await webApiClient.For<ODataCohort>()
                .Key(activeCohortId)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);
                odataproject.Cohort = cohort;
            }
            if (project.ModuleId == null)
            {
                odataproject.Module = null;
            }
            if (project.MainProjectId == null)
            {
                odataproject.MainProject = null;
            }
            if (project.TopicId == null)
            {
                odataproject.Topic = null;
            }

            await webApiClient.For<ODataProject>()
                .Key(odataproject)
                .Set(odataproject)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IList<Project>> ListProjectsByTopicIdAsync(Guid Id, Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProjects = await webApiClient.For<ODataProject>()
                .Filter(c => c.Topic.TopicId == Id)
                .Filter(c => c.Applicant.ContactId != userId)
                .OrderBy(x => x.StartDate)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Project>>(odataProjects);
        }

        public async Task<IList<Project>> ListLeadProjectsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProjects = await webApiClient.For<ODataProject>()
                .Filter(c => c.Applicant.ContactId == userId)
                .Filter(c => c.TeamLead == true)
                .OrderBy(x => x.StartDate)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Project>>(odataProjects);
        }

        public async Task<IList<Project>> ListParentProjectsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProjects = await webApiClient.For<ODataProject>()
                .Filter(c => c.Applicant.ContactId == userId)
                //.Filter(c => c.ProjectStatus != 5) // Not Completed
                .Filter(c => c.TeamLead == false)
                .Filter(c => c.MainProject != null)
                .OrderBy(x => x.StartDate)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            List<Project> ParentProjects = new List<Project> { };
            foreach (var odataProject in odataProjects)
            {
                var parent = await GetProjectByIdAsync(odataProject.MainProject.Id);
                if (parent.Value.ProjectStatus != 5)// Not Completed
                {
                    ParentProjects.Add(parent.Value);
                }
            }
            return this.mapper.Map<IList<Project>>(ParentProjects);
        }
    }
}