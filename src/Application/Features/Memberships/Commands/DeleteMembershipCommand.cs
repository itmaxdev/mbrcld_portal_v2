using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Memberships.Commands
{
    public class DeleteMembershipCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public DeleteMembershipCommand(Guid id)
        {
            this.Id = id;
        }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<DeleteMembershipCommand, Result>
        {
            private readonly IMembershipRepository MembershipRepository;

            public CommandHandler(IMembershipRepository MembershipRepository)
            {
                this.MembershipRepository = MembershipRepository;
            }

            public async Task<Result> Handle(DeleteMembershipCommand request, CancellationToken cancellationToken)
            {
                await MembershipRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
