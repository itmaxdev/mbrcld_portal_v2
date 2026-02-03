using Mbrcld.Application.Interfaces;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Accounts.Commands
{
    public sealed class ResetPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<ResetPasswordCommand, Result>
        {
            private readonly IAccountService accountService;

            public CommandHandler(IAccountService accountService)
            {
                this.accountService = accountService;
            }

            public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                return await this.accountService.SendPasswordResetLinkAsync(request.Email, cancellationToken);
            }
        }
        #endregion
    }
}
