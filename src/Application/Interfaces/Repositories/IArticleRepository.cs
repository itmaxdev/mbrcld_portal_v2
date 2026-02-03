using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IArticleRepository
    {
        Task<IList<Article>> ListArticlesAsync(CancellationToken cancellationToken = default);
        Task<IList<Article>> ListUserArticlesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Maybe<Article>> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<Article>> SearchArticlesAsync(string search, CancellationToken cancellationToken = default);
        Task<IList<Article>> SearchUserArticlesAsync(Guid id, string search, CancellationToken cancellationToken = default);
        Task UpdateAsync(Article article, CancellationToken cancellationToken = default);
        Task<Maybe<Article>> CreateAsync(Article article, CancellationToken cancellationToken = default);
    }
}
