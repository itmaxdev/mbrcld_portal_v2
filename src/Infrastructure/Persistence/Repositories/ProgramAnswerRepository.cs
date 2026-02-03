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
    internal sealed class ProgramAnswerRepository : IProgramAnswerRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public ProgramAnswerRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<IList<ProgramAnswer>> ListByEnrollmentIdAsync(Guid enrollmentId, CancellationToken cancellationToken = default)
        {
            var odataProgramAnswers = await webApiClient.For<ODataProgramAnswer>()
                .Filter(c => c.Enrollment.Id == enrollmentId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<ProgramAnswer>>(odataProgramAnswers);
        }

        public async Task<Result> CreateAsync(ProgramAnswer programAnswser, CancellationToken cancellationToken = default)
        {
            var odataProgramAnswers = this.mapper.Map<ODataProgramAnswer>(programAnswser);

            await webApiClient.For<ODataProgramAnswer>()
                .Set(odataProgramAnswers)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }

        public async Task UpdateAsync(ProgramAnswer programAnswser, CancellationToken cancellationToken = default)
        {
            var odataProgramAnswers = this.mapper.Map<ODataProgramAnswer>(programAnswser);

            await webApiClient.For<ODataProgramAnswer>()
                .Key(odataProgramAnswers)
                .Set(odataProgramAnswers)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<ProgramAnswer>> GetAsync(Guid enrollmentId, Guid questionId, CancellationToken cancellationToken = default)
        {
            var odataProgramAnswers = await webApiClient.For<ODataProgramAnswer>()
                .Filter(x => x.Enrollment.Id == enrollmentId)
                .Filter(x => x.QuestionId.Id == questionId)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<ProgramAnswer>(odataProgramAnswers);
        }
    }
}
