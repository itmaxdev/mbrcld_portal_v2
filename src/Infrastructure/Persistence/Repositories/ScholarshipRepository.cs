using AutoMapper;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class ScholarshipRepository : IScholarshipRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public ScholarshipRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper
            )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public ScholarshipRepository()
        {
        }

        public async Task<Maybe<Scholarship>> GetScholarshipByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataScholarship = await webApiClient.For<ODataScholarship>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Scholarship>(odataScholarship);
        }
        public async Task<IList<Scholarship>> GetScholarshipAsync(CancellationToken cancellationToken = default)
        {
            var odataScholarships = await this.webApiClient.For<ODataScholarship>()
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Scholarship>>(odataScholarships);
        }
    }
}
