using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface ITopicRepository
    {
        Task<IList<Topic>> ListTopicsByProgramIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Maybe<Topic>> GetTopicByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
