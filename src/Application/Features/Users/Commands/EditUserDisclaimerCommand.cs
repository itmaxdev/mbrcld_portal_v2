using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class EditUserDisclaimerCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public int DisclaimerType { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditUserDisclaimerCommand, Result>
        {
            private readonly IUserRepository userRepository;

            public CommandHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }

            public async Task<Result> Handle(EditUserDisclaimerCommand request, CancellationToken cancellationToken)
            {
                var user = (User)await userRepository.GetByIdAsync(request.Id, cancellationToken);

                if (request.DisclaimerType == 1)//ArticlesDisclaimer
                {
                    user.ArticlesDisclaimer = true;
                }
                if (request.DisclaimerType == 2)//IdeaHubDisclaimer
                {
                    user.IdeaHubDisclaimer = true;
                }
                if (request.DisclaimerType == 3)//SpecialProjectDisclaimer
                {
                    user.SpecialProjectDisclaimer = true;
                }
                await userRepository.UpdateAsync(user);

                return Result.Success();
            }
        }
        #endregion
    }
}
