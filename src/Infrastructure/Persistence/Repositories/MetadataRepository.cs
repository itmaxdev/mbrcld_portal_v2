using AutoMapper;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class MetadataRepository : IMetadataRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;
        private readonly IPreferredLanguageService languageService;

        public MetadataRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper,
            IPreferredLanguageService languageService
            )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
            this.languageService = languageService;
        }

        public async Task<IList<Industry>> GetIndustriesAsync(CancellationToken cancellationToken = default)
        {
            var odataIndustries = (await this.webApiClient.For<ODataIndustry>()
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false))
                .ToList();

            if (this.languageService.GetPreferredLanguageLCID() == 1025 /*Arabic*/)
            {
                foreach (var item in odataIndustries)
                {
                    item.Name = item.Name_Ar;
                }
            }

            return this.mapper.Map<IList<Industry>>(odataIndustries);
        }

        public async Task<IList<Language>> GetLanguagesAsync(CancellationToken cancellationToken = default)
        {
            var odataLanguages = (await this.webApiClient.For<ODataLanguage>()
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false))
                .ToList();

            if (this.languageService.GetPreferredLanguageLCID() == 1025 /*Arabic*/)
            {
                foreach (var item in odataLanguages)
                {
                    item.Name = item.Name_Ar;
                }
            }

            return this.mapper.Map<IList<Language>>(odataLanguages);
        }

        public async Task<IList<Sector>> GetSectorsAsync(CancellationToken cancellationToken = default)
        {
            var odataSectors = (await this.webApiClient.For<ODataSector>()
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false))
                .ToList();

            if (this.languageService.GetPreferredLanguageLCID() == 1025 /*Arabic*/)
            {
                foreach (var item in odataSectors)
                {
                    item.Name = item.Name_Ar;
                }
            }

            return this.mapper.Map<IList<Sector>>(odataSectors);
        }
    }
}
