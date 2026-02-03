using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IEventRegistrantRepository
    {
        Task<Result> CreateAsync(EventRegistrant eventregistrant, CancellationToken cancellationToken = default);
        Task<Maybe<EventRegistrant>> GetEventRegistrantByUserIdAndEventIdAsync(Guid userId, Guid eventid, CancellationToken cancellationToken = default);
        Task<int> ListEventRegistrantByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<EventRegistrant>> EventRegistrantByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    }
}
