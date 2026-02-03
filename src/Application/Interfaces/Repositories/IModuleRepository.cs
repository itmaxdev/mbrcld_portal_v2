using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IModuleRepository
    {
        Task<IList<Module>> ListModulesByProgramIdAsync(Guid programId, Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Module>> ListModulesByEliteClubIdAsync(Guid eliteClubId, Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Module>> ListModulesByCohortIdAsync(Guid cohortId, CancellationToken cancellationToken = default);
        Task<IList<User>> ListModuleApplicantsAsync(Guid moduleId, CancellationToken cancellationToken = default);
        Task<IList<Module>> ListUserModulesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<int> ListModulesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Module> GetModuleByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User> GetApplicantProfileAsync(Guid id, CancellationToken cancellationToken = default);
        Task<University> GetUniversityProfileAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Module module, CancellationToken cancellationToken = default);
        Task<Module> GetCurrentModuleAsync(Guid userId, CancellationToken cancellationToken = default);

        
    }
}
