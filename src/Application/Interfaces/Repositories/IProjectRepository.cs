using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<IList<Project>> ListProjectsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Project>> ListCompletedProjectsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Project>> ListProjectsByCohortIdAsync(Guid cohortId, Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Project>> ListLeadProjectsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Project>> ListParentProjectsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Project>> ListInstructorProjectsAsync(Guid userId, Guid moduleId, CancellationToken cancellationToken = default);
        Task<IList<Project>> GetProjectAsync(Guid moduleId, Guid userId, CancellationToken cancellationToken = default);
        Task<Result> CreateInstructorProjectAsync(Project project, CancellationToken cancellationToken = default);
        Task<Maybe<Project>> GetProjectByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Project project, CancellationToken cancellationToken = default);
        Task<IList<Project>> ListProjectsByTopicIdAsync(Guid Id, Guid userId, CancellationToken cancellationToken = default);
    }
}
