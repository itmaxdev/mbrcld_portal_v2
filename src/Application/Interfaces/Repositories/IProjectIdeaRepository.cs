using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IProjectIdeaRepository
    {
        Task<IList<ProjectIdea>> ListProjectIdeasAsync(CancellationToken cancellationToken = default);
        Task<IList<ProjectIdea>> ListUserProjectIdeasAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Maybe<ProjectIdea>> GetProjectIdeaByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<ProjectIdea>> SearchProjectIdeasAsync(string search, CancellationToken cancellationToken = default);
        Task UpdateAsync(ProjectIdea projectidea, CancellationToken cancellationToken = default);
        Task<Maybe<ProjectIdea>> CreateAsync(ProjectIdea projectidea, CancellationToken cancellationToken = default);
    }
}
