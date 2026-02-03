using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        Task<Maybe<Country>> GetByIsoCodeAsync(string countryIsoCode, CancellationToken cancellationToken = default);
    }
}
