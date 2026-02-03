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
    internal sealed class UniversityRepository : IUniversityRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UniversityRepository(
            ISimpleWebApiClient webApiClient,
            IUserRepository userRepository,
            IMapper mapper
            )
        {
            this.webApiClient = webApiClient;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Maybe<University>> GetUniversityByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user.Value.Role == 4) // Instructor
            {
                if(user.Value.UniversityId != null)
                {
                    var odataUniversity = await webApiClient.For<ODataUniversity>()
                    .Key(user.Value.UniversityId)
                    .ProjectToModel()
                    .FindEntryAsync(cancellationToken)
                    .ConfigureAwait(false);
                    return this.mapper.Map<University>(odataUniversity);
                }
            }
            return null;
        }

        public async Task<Maybe<University>> GetUniversityAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataUniversity = await webApiClient.For<ODataUniversity>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<University>(odataUniversity);
        }

        public async Task UpdateAsync(University University, CancellationToken cancellationToken = default)
        {
            var odataUniversity = this.mapper.Map<ODataUniversity>(University);

            await this.webApiClient.For<ODataUniversity>()
                .Key(odataUniversity)
                .Set(odataUniversity)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
