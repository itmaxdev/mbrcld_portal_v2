using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class EditUserProfileUpdateOnCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        
        public DateTime? ProfileUpdateOn { get; set; }

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditUserProfileUpdateOnCommand, Result>
        {
            private readonly IUserRepository userRepository;

            public CommandHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }

            public async Task<Result> Handle(EditUserProfileUpdateOnCommand request, CancellationToken cancellationToken)
            {
                var user = (User)await userRepository.GetByIdAsync(request.Id, cancellationToken);
                user.ProfileUpdateOn = request.ProfileUpdateOn;

                await userRepository.UpdateAsync(user);
                return Result.Success();
            }
        }
        #endregion
    }
}
