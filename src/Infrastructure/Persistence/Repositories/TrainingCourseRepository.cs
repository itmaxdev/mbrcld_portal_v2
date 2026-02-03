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
    internal sealed class TrainingCourseRepository: ITrainingCourseRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public TrainingCourseRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(TrainingCourse trainingCourse, CancellationToken cancellationToken = default)
        {
            var odataTraining = this.mapper.Map<ODataTrainingCourse>(trainingCourse);

           await webApiClient.For<ODataTrainingCourse>()
                .Set(odataTraining)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success(trainingCourse);
        }

        public async Task UpdateAsync(TrainingCourse trainingCourse, CancellationToken cancellationToken = default)
        {
            var odataTrainingCourse = this.mapper.Map<ODataTrainingCourse>(trainingCourse);

            await this.webApiClient.For<ODataTrainingCourse>()
                .Key(odataTrainingCourse)
                .Set(odataTrainingCourse)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For<ODataTrainingCourse>()
                .Key(id)
                .DeleteEntryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<TrainingCourse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataTrainingCourse = await webApiClient.For<ODataTrainingCourse>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<TrainingCourse>(odataTrainingCourse);
        }

        public async Task<IList<TrainingCourse>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataTrainingCourses = await this.webApiClient.For<ODataTrainingCourse>()
                .Filter(c => c.ContactId.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<TrainingCourse>>(odataTrainingCourses);
        }
    }
}
