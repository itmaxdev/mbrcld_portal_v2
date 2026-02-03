using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<IList<Post>> ListPostsAsync(CancellationToken cancellationToken = default);
        Task<Post> PinnedPostsAsync(CancellationToken cancellationToken = default);
        
        Task<IList<Post>> SearchPostsAsync(string search, CancellationToken cancellationToken = default);
        Task<Maybe<Post>> GetPostByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
