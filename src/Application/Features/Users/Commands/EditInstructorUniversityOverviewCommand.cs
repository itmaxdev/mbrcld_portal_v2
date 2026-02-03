using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class EditInstructorUniversityOverviewCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string AboutUniversity { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditInstructorUniversityOverviewCommand, Result>
        {
            private readonly IUserRepository userRepository;
            private readonly ICountryRepository countryRepository;

            public CommandHandler(IUserRepository userRepository, ICountryRepository countryRepository)
            {
                this.userRepository = userRepository;
                this.countryRepository = countryRepository;
            }

            public async Task<Result> Handle(EditInstructorUniversityOverviewCommand request, CancellationToken cancellationToken)
            {
                var user = (User)await userRepository.GetByIdAsync(request.Id, cancellationToken);

                user.RepresentedUniversityIntroduction = request.AboutUniversity;

                await userRepository.UpdateAsync(user);

                return Result.Success();
            }
        }
        #endregion
    }
}
