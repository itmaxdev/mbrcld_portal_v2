using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class SpecialProjectRepository : ISpecialProjectRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public SpecialProjectRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Maybe<SpecialProject>> CreateAsync(SpecialProject specialProject, CancellationToken cancellationToken = default)
        {
            var odataSpecialProject = this.mapper.Map<ODataSpecialProject>(specialProject);
            if (specialProject.SpecialProjectTopicId == Guid.Empty)
            {
                odataSpecialProject.SpecialProjectTopic = null;
            }
            if (specialProject.SectorId == Guid.Empty)
            {
                odataSpecialProject.Sector = null;
            }
            odataSpecialProject.CreatedOn = DateTime.UtcNow;

            await webApiClient.For<ODataSpecialProject>()
                .Set(odataSpecialProject)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<SpecialProject>(odataSpecialProject);
        }

        public async Task UpdateAsync(SpecialProject specialProject, CancellationToken cancellationToken = default)
        {
            var odataSpecialProject = this.mapper.Map<ODataSpecialProject>(specialProject);
            if (specialProject.SpecialProjectTopicId == Guid.Empty)
            {
                odataSpecialProject.SpecialProjectTopic = null;
            }
            if (specialProject.SectorId == Guid.Empty)
            {
                odataSpecialProject.Sector = null;
            }

            await webApiClient.For<ODataSpecialProject>()
                .Key(odataSpecialProject)
                .Set(odataSpecialProject)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<SpecialProject>> GetSpecialProjectByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataSpecialProject = await webApiClient.For<ODataSpecialProject>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<SpecialProject>(odataSpecialProject);
        }

        //public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        //{
        //    await webApiClient.For<ODataEnrollment>()
        //        .Key(id)
        //        .DeleteEntryAsync(cancellationToken)
        //        .ConfigureAwait(false);
        //}

        public async Task<IList<SpecialProject>> ListSpecialProjectsAsync(CancellationToken cancellationToken = default)
        {
            var odataSpecialProjects = await webApiClient.For<ODataSpecialProject>()
                .Filter(c => c.SpecialProjectStatus == 3) //Published
                .OrderByDescending(x => x.Date)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<SpecialProject>>(odataSpecialProjects);
        }

        public async Task<IList<SpecialProject>> ListUserSpecialProjectsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataSpecialProjects = await webApiClient.For<ODataSpecialProject>()
                .Filter(c => c.AlumniId.ContactId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<SpecialProject>>(odataSpecialProjects);
        }
        public async Task<IList<SpecialProject>> SearchSpecialProjectsAsync(string search, CancellationToken cancellationToken = default)
        {
            var odataSpecialProjects = await webApiClient.For<ODataSpecialProject>()
                .Filter(c => c.Name.Contains(search) || c.Desription.Contains(search))
                .Filter(c => c.SpecialProjectStatus == 3) //Published
                .OrderByDescending(x => x.Date)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<SpecialProject>>(odataSpecialProjects);
        }
    }
}