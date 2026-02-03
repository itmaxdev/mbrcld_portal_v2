using Mbrcld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IMetadataRepository
    {
        Task<IList<Industry>> GetIndustriesAsync(CancellationToken cancellationToken = default);
        Task<IList<Sector>> GetSectorsAsync(CancellationToken cancellationToken = default);
        Task<IList<Language>> GetLanguagesAsync(CancellationToken cancellationToken = default);
    }
}
