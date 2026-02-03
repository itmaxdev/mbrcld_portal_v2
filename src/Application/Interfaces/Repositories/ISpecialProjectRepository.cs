using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface ISpecialProjectRepository
    {
        Task<IList<SpecialProject>> ListSpecialProjectsAsync(CancellationToken cancellationToken = default);
        Task<IList<SpecialProject>> ListUserSpecialProjectsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Maybe<SpecialProject>> GetSpecialProjectByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<SpecialProject>> SearchSpecialProjectsAsync(string search, CancellationToken cancellationToken = default);
        Task UpdateAsync(SpecialProject specialProject, CancellationToken cancellationToken = default);
        Task<Maybe<SpecialProject>> CreateAsync(SpecialProject specialProject, CancellationToken cancellationToken = default);
    }
}
