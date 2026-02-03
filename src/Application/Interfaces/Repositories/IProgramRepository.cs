using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IProgramRepository
    {
        Task<IList<Program>> ListProgramsByUserModulesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Program>> ListAllProgramsAsync(CancellationToken cancellationToken = default);
        Task<IList<Program>> ListActiveProgramsAsync(CancellationToken cancellationToken = default);
        Task<IList<int>> ListDistinctCohortYearsAsync(Guid programId,CancellationToken cancellationToken = default);
        Task<IList<Program>> ListProgramsByCohortContactAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Program>> ListAlumniAvailableProgramAsync(Guid userId, string year, CancellationToken cancellationToken = default);
        Task<IList<Cohort>> ListAlumniGraduatedProgramAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Program>> ListUserProgramsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Program> GetProgramByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Program> GetProgramDetailsByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
        Task<Program> GetActiveProgramAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Program> GetCurrentActiveProgramAsync(CancellationToken cancellationToken = default);
        
    }
}
