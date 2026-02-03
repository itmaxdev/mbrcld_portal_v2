using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.TrainingCourses.Commands
{
    public class RemoveTrainingCourseCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public RemoveTrainingCourseCommand(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<RemoveTrainingCourseCommand, Result>
        {
            private readonly ITrainingCourseRepository trainingCourseRepository;

            public CommandHandler(ITrainingCourseRepository trainingCourseRepository)
            {
                this.trainingCourseRepository = trainingCourseRepository;
            }

            public async Task<Result> Handle(RemoveTrainingCourseCommand request, CancellationToken cancellationToken)
            {
                await trainingCourseRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
