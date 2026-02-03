using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IContentRepository
    {
        Task<IList<Content>> ListContentsBySectionIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Content> GetContentByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Content content, CancellationToken cancellationToken = default);
        Task<Maybe<Content>> CreateAsync(Content content, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
