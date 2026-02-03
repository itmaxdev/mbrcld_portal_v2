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
    internal sealed class SurveyRepository : ISurveyRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public SurveyRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper
            )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<IList<Survey>> ListUserSurveysUrlAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataSurveys = await this.webApiClient.For<ODataSurvey>()
                .Filter(c => c.Contact.ContactId == userId)
                .Filter(c => c.Status == 1) //Pending
                                            //.OrderBy(x => x.SurveyTemplate.Name)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Survey>>(odataSurveys);
        }

        public async Task<IList<Survey>> ListSurveysByProgramIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataSurveys = await this.webApiClient.For<ODataSurvey>()
                .Filter(c => c.Contact.ContactId == userId)
                .Filter(c => c.Program.Id != null)
                .Filter(c => c.Status == 1) //Pending
                                            //.OrderBy(x => x.SurveyTemplate.Name)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Survey>>(odataSurveys);
        }

        public async Task<IList<Survey>> ListUserCompletedSurveysAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataSurveys = await this.webApiClient.For<ODataSurvey>()
                .Filter(c => c.Contact.ContactId == userId)
                .Filter(c => c.Status == 936510000) //Completed
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Survey>>(odataSurveys);
        }
    }
}
