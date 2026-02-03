using AutoMapper;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class MentorRepository : IMentorRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public MentorRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper
            )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Maybe<Mentor>> GetMentorByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataMentor = await webApiClient.For<ODataMentor>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Mentor>(odataMentor);
        }
    }
}
