using FluentValidation;
using Mbrcld.Application.Interfaces;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Accounts.Commands
{
    public sealed class ChangePasswordCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<ChangePasswordCommand, Result>
        {
            private readonly IAccountService accountService;

            public CommandHandler(IAccountService accountService)
            {
                this.accountService = accountService;
            }

            public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                return await this.accountService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword, cancellationToken);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<ChangePasswordCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.CurrentPassword).NotEmpty();
                RuleFor(x => x.NewPassword).NotEmpty();
            }
        }
        #endregion
    }
}
