using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.ProfessionalExperiences.Commands
{
    public class RemoveProfessionalExperienceCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public RemoveProfessionalExperienceCommand(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<RemoveProfessionalExperienceCommand, Result>
        {
            private readonly IProfessionalExperienceRepository professionalExperienceRepository;

            public CommandHandler(IProfessionalExperienceRepository professionalExperienceRepository)
            {
                this.professionalExperienceRepository = professionalExperienceRepository;
            }

            public async Task<Result> Handle(RemoveProfessionalExperienceCommand request, CancellationToken cancellationToken)
            {
                await professionalExperienceRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
