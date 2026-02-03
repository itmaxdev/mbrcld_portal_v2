using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IMaterialRepository
    {
        Task<IList<Material>> ListMaterialsByModuleIdAsync(Guid Id, Guid UserId, CancellationToken cancellationToken = default);
        Task<IList<Material>> ListMaterialsByCohortModuleIdAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<Material> GetMaterialByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Material material, CancellationToken cancellationToken = default);
        Task<Maybe<Material>> CreateAsync(Material material, CancellationToken cancellationToken = default);
    }
}
