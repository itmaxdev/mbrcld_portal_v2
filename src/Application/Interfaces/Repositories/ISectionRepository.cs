using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface ISectionRepository
    {
        Task<IList<Section>> ListSectionsByMaterialIdAsync(Guid Id, Guid userId, CancellationToken cancellationToken = default);
        Task<IList<Section>> ListSectionsByCohortMaterialIdAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<Section> GetSectionByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
        Task UpdateSectionStatusAsync(Guid id, Guid userId, int status, CancellationToken cancellationToken = default);
        Task UpdateAsync(Section section, CancellationToken cancellationToken = default);
        Task<Maybe<Section>> CreateAsync(Section section, CancellationToken cancellationToken = default);
        Task<int> ListUnreadSectionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<int> ListReviewSectionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<int> ListDoneSectionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
