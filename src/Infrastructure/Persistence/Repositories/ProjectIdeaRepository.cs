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
    internal sealed class ProjectIdeaRepository : IProjectIdeaRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public ProjectIdeaRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Maybe<ProjectIdea>> CreateAsync(ProjectIdea projectidea, CancellationToken cancellationToken = default)
        {
            var odataProjectIdea = this.mapper.Map<ODataProjectIdea>(projectidea);
            if (projectidea.SectorId == Guid.Empty)
            {
                odataProjectIdea.Sector = null;
            }
            odataProjectIdea.CreatedOn = DateTime.UtcNow;

            await webApiClient.For<ODataProjectIdea>()
                .Set(odataProjectIdea)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<ProjectIdea>(odataProjectIdea);
        }

        public async Task UpdateAsync(ProjectIdea projectidea, CancellationToken cancellationToken = default)
        {
            var odataProjectIdea = this.mapper.Map<ODataProjectIdea>(projectidea);
            if (projectidea.SectorId == Guid.Empty)
            {
                odataProjectIdea.Sector = null;
            }

            await webApiClient.For<ODataProjectIdea>()
                .Key(odataProjectIdea)
                .Set(odataProjectIdea)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<ProjectIdea>> GetProjectIdeaByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataProjectIdea = await webApiClient.For<ODataProjectIdea>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<ProjectIdea>(odataProjectIdea);
        }

        //public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        //{
        //    await webApiClient.For<ODataEnrollment>()
        //        .Key(id)
        //        .DeleteEntryAsync(cancellationToken)
        //        .ConfigureAwait(false);
        //}

        public async Task<IList<ProjectIdea>> ListProjectIdeasAsync(CancellationToken cancellationToken = default)
        {
            var odataProjectIdeas = await webApiClient.For<ODataProjectIdea>()
                .Filter(c => c.ProjectIdeaStatus == 3) //Published
                .OrderByDescending(x => x.Date)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<ProjectIdea>>(odataProjectIdeas);
        }

        public async Task<IList<ProjectIdea>> ListUserProjectIdeasAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProjectIdeas = await webApiClient.For<ODataProjectIdea>()
                .Filter(c => c.AlumniId.ContactId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<ProjectIdea>>(odataProjectIdeas);
        }
        public async Task<IList<ProjectIdea>> SearchProjectIdeasAsync(string search, CancellationToken cancellationToken = default)
        {
            var odataProjectIdeas = await webApiClient.For<ODataProjectIdea>()
                .Filter(c => c.Name.Contains(search) || c.Desription.Contains(search))
                .Filter(c => c.ProjectIdeaStatus == 3) //Published
                .OrderByDescending(x => x.Date)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<ProjectIdea>>(odataProjectIdeas);
        }
    }
}