using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class ContentRepository : IContentRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public ContentRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<IList<Content>> ListContentsBySectionIdAsync(Guid sectionId, CancellationToken cancellationToken = default)
        {
            var odatacontents = await webApiClient.For<ODataContent>()
                .Filter(p => p.Section.Id == sectionId)
                .OrderBy(o =>o.Order)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Content>>(odatacontents);
        }

        public async Task<Content> GetContentByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odatacontent = await webApiClient.For<ODataContent>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Content>(odatacontent);
        }

        public async Task<Maybe<Content>> CreateAsync(Content content, CancellationToken cancellationToken = default)
        {
            var odatacontent = this.mapper.Map<ODataContent>(content);
            //odataArticle.CreatedOn = DateTime.UtcNow;

            await webApiClient.For<ODataContent>()
                .Set(odatacontent)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Content>(odatacontent);
        }

        public async Task UpdateAsync(Content content, CancellationToken cancellationToken = default)
        {
            var odatacontent = this.mapper.Map<ODataContent>(content);

            await webApiClient.For<ODataContent>()
                .Key(odatacontent)
                .Set(odatacontent)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For<ODataContent>()
                .Key(id)
                .DeleteEntryAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}