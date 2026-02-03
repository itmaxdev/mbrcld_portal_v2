using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IEventRepository
    {
        Task<Maybe<Event>> GetEventByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> GetEventAvailabilityAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<Event>> GetEventsAsync(Guid UserId,CancellationToken cancellationToken = default);
    }
}
