using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class SetIsActiveMemberCommand : IRequest<Result>
    {
        #region Command
        public Guid UserId { get; set; }
        public bool? IsActiveMember { get; set; }
        #endregion

        #region Command Handler
        private sealed class CommandHandler : IRequestHandler<SetIsActiveMemberCommand, Result>
        {
            private readonly IUserRepository userRepository;

            public CommandHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }

            public async Task<Result> Handle(SetIsActiveMemberCommand request, CancellationToken cancellationToken)
            {
                var user = (User)await this.userRepository.GetByIdAsync(request.UserId, cancellationToken);
                user.IsActiveMemberInBoardOrInstitution = request.IsActiveMember.Value;
                await this.userRepository.UpdateAsync(user, cancellationToken);
                return Result.Success();
            }
        }
        #endregion

        #region Command Validator
        public sealed class CommandValidator : AbstractValidator<SetIsActiveMemberCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.IsActiveMember).NotEmpty();
            }
        }
        #endregion
    }
}
