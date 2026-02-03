using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface ICalendarRepository
    {
        Task<IList<Calendar>> GetUserCalendarAsync(Guid userId ,CancellationToken cancellationToken = default);
        Task<IList<Calendar>> ListApplicantMeetingAsync(Guid userId ,CancellationToken cancellationToken = default);
    }
}
