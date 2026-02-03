using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.SkillsAndInterests.Commands
{
    public class RemoveInterestCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public RemoveInterestCommand(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<RemoveInterestCommand, Result>
        {
            private readonly IInterestRepository interestRepository;

            public CommandHandler(IInterestRepository interestRepository)
            {
                this.interestRepository = interestRepository;
            }

            public async Task<Result> Handle(RemoveInterestCommand request, CancellationToken cancellationToken)
            {
                await interestRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
