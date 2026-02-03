using Dawn;
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
    public sealed class EditUserLearningPreferencesCommand : IRequest<Result>
    {
        #region Command
        public Guid UserId { get; set; }
        public int[] SelectedValues { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditUserLearningPreferencesCommand, Result>
        {
            private readonly IUserRepository userRepository;

            public CommandHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }

            public async Task<Result> Handle(EditUserLearningPreferencesCommand request, CancellationToken cancellationToken)
            {
                var user = (User)await userRepository.GetByIdAsync(request.UserId, cancellationToken);
                user.LearningPreferences = request.SelectedValues;
                await userRepository.UpdateAsync(user);
                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        private sealed class CommandValidator : AbstractValidator<EditUserLearningPreferencesCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.SelectedValues).NotNull();
            }
        }
        #endregion
    }
}
