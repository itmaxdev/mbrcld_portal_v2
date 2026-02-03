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
    internal sealed class EventAnswersRepository : IEventAnswersRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public EventAnswersRepository(
            ISimpleWebApiClient webApiClient, 
            IMapper mapper
           )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(EventAnswer eventAnswers, CancellationToken cancellationToken = default)
        {
            var odataEventAnswers = this.mapper.Map<ODataEventAnswers>(eventAnswers);

            await webApiClient.For<ODataEventAnswers>()
                .Set(odataEventAnswers)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }
    }
}