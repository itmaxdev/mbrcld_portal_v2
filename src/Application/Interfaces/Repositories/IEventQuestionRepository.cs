using Mbrcld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IEventQuestionRepository
    {
        Task<IList<EventQuestion>> ListByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
    }
}
