using FluentValidation;
using Mbrcld.Application.Interfaces;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Accounts.Commands
{
    public sealed class ChangeEmailCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public string NewEmail { get; set; }

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<ChangeEmailCommand, Result>
        {
            private readonly IAccountService accountService;

            public CommandHandler(IAccountService accountService)
            {
                this.accountService = accountService;
            }

            public async Task<Result> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
            {
                var user = await this.accountService.GetByEmailAsync(request.NewEmail);
                if (user.HasValue)
                {
                    return Result.Failure("DuplicateEmail");
                }

                return await this.accountService.SendChangeEmailConfirmationLinkAsync(request.UserId, request.NewEmail, cancellationToken);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<ChangeEmailCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.NewEmail).NotEmpty();
            }
        }
        #endregion
    }
}
