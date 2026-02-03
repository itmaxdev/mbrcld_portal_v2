using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface ISpecialProjectTopicRepository
    {
        Task<IList<SpecialProjectTopic>> ListSpecialProjectTopicsAsync(CancellationToken cancellationToken = default);
    }
}
