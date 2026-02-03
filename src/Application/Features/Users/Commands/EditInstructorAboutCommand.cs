using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class EditInstructorAboutCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string AboutInstructor { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditInstructorAboutCommand, Result>
        {
            private readonly IUserRepository userRepository;
            private readonly ICountryRepository countryRepository;

            public CommandHandler(IUserRepository userRepository, ICountryRepository countryRepository)
            {
                this.userRepository = userRepository;
                this.countryRepository = countryRepository;
            }

            public async Task<Result> Handle(EditInstructorAboutCommand request, CancellationToken cancellationToken)
            {
                var user = (User)await userRepository.GetByIdAsync(request.Id, cancellationToken);

                user.AboutInstructor = request.AboutInstructor;

                await userRepository.UpdateAsync(user);

                return Result.Success();
            }
        }
        #endregion
    }
}
