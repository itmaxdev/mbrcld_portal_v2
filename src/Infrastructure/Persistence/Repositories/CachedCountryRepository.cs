using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal class CachedCountryRepository : ICountryRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;

        public CachedCountryRepository(ISimpleWebApiClient webApiClient, IMapper mapper, IMemoryCache memoryCache)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
        }

        public async Task<Maybe<Country>> GetByIsoCodeAsync(string countryIsoCode, CancellationToken cancellationToken = default)
        {
            var allCountries = await this.GetAllCountriesAsync();
            var country = allCountries.FirstOrDefault(c => c.IsoCode == countryIsoCode);
            return this.mapper.Map<Country>(country);
        }

        private async Task<IReadOnlyList<ODataCountry>> GetAllCountriesAsync()
        {
            return await this.memoryCache.GetOrCreateAsync(CacheKey, async (e) =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

                var countries = await this.webApiClient.For<ODataCountry>()
                    .ProjectToModel()
                    .FindEntriesAsync();
                
                return countries.ToList().AsReadOnly();
            });
        }

        private static string CacheKey => typeof(CachedCountryRepository).FullName;
    }
}
