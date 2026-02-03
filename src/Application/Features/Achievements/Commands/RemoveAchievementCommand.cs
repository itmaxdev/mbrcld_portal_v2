using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Achievements.Commands
{
    public class RemoveAchievementCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public RemoveAchievementCommand(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<RemoveAchievementCommand, Result>
        {
            private readonly IAchievementRepository achievementRepository;

            public CommandHandler(IAchievementRepository achievementRepository)
            {
                this.achievementRepository = achievementRepository;
            }

            public async Task<Result> Handle(RemoveAchievementCommand request, CancellationToken cancellationToken)
            {
                await achievementRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
