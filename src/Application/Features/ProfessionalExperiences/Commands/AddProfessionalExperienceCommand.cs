using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.ProfessionalExperiences.Commands
{
    public sealed class AddProfessionalExperienceCommand : IRequest<Result>
    {
        #region Command
        public Guid ContactId { get; set; }
        public string JobTitle { get; set; }
        public string JobTitle_AR { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationName_AR { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public Guid Industry { get; set; }
        public Guid Sector { get; set; }
        public string OtherIndustry { get; set; }
        public string OtherSector { get; set; }
        public int? OrganizationSize { get; set; }
        public int? PositionLevel { get; set; }
        public int? OrganizationLevel { get; set; }
        public bool Completed { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddProfessionalExperienceCommand, Result>
        {
            private readonly IProfessionalExperienceRepository professionalExperienceRepository;

            public CommandHandler(IProfessionalExperienceRepository professionalExperienceRepository)
            {
                this.professionalExperienceRepository = professionalExperienceRepository;
            }

            public async Task<Result> Handle(AddProfessionalExperienceCommand request, CancellationToken cancellationToken)
            {
                var professionalExperience = ProfessionalExperience.Create(
                    contact: request.ContactId,
                    industry: request.Industry,
                    sector: request.Sector,
                    organizationName: request.OrganizationName,
                    organizationName_Ar: request.OrganizationName_AR,
                    jobTitle: request.JobTitle,
                    jobTitle_Ar: request.JobTitle_AR,
                    from: request.From,
                    to: request.To,
                    otherIndustry: request.OtherIndustry,
                    otherSector: request.OtherSector,
                    organizationSize: request.OrganizationSize,
                    PositionLevel: request.PositionLevel,
                    OrganizationLevel: request.OrganizationLevel
                    );

                await professionalExperienceRepository.CreateAsync(professionalExperience, cancellationToken).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddProfessionalExperienceCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Industry).NotNull().NotEmpty();
                RuleFor(x => x.Sector).NotNull().NotEmpty();
                RuleFor(x => x.OrganizationName).NotNull().NotEmpty();
                RuleFor(x => x.OrganizationName_AR).NotNull().NotEmpty();
                RuleFor(x => x.JobTitle).NotNull().NotEmpty();
                RuleFor(x => x.JobTitle_AR).NotNull().NotEmpty();
                RuleFor(x => x.From).NotNull().NotEmpty();
                RuleFor(x => x.OrganizationSize).NotEmpty();
                //RuleFor(x => x.PositionLevel).<PositionLevelEnum>();
                //RuleFor(x => x.OrganizationLevel).NotEmpty();
            }
        }
        #endregion
    }
}
