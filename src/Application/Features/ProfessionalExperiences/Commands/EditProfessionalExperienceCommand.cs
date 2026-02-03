using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.ProfessionalExperiences.Commands
{
    public sealed class EditProfessionalExperienceCommand : IRequest
    {
        #region Command
        public Guid Id { get; set; }
        public string JobTitle { get; set; }
        public string OrganizationName { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public Guid Industry { get; set; }
        public Guid Sector { get; set; }
        public string OtherIndustry { get; set; }
        public string OtherSector { get; set; }
        public string JobTitle_AR { get; set; }
        public string OrganizationName_AR { get; set; }
        public int? OrganizationSize { get; set; }
        public int? PositionLevel { get; set; }
        public int? OrganizationLevel { get; set; }
        public bool Completed { get; set;}
        #endregion

        #region Command handler
        public sealed class CommandHandler : AsyncRequestHandler<EditProfessionalExperienceCommand>
        {
            private readonly IProfessionalExperienceRepository professionalExperienceRepository;

            public CommandHandler(IProfessionalExperienceRepository professionalExperienceRepository)
            {
                this.professionalExperienceRepository = professionalExperienceRepository;
            }

            protected override async Task Handle(EditProfessionalExperienceCommand request, CancellationToken cancellationToken)
            {
                var profExperience = await professionalExperienceRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (profExperience.HasNoValue)
                {
                    throw new Exception();
                }

                var pfValue = profExperience.Value;

                pfValue.Industry = request.Industry;
                pfValue.Sector = request.Sector;
                pfValue.JobTitle = request.JobTitle;
                pfValue.JobTitle_Ar = request.JobTitle_AR;
                pfValue.OrganizationName = request.OrganizationName;
                pfValue.OrganizationName_Ar = request.OrganizationName_AR;
                pfValue.From = request.From;
                pfValue.To = request.To;
                pfValue.OrganizationSize = request.OrganizationSize;
                pfValue.PositionLevel = request.PositionLevel; //Enum.GetName(typeof(PositionLevelEnum), request.PositionLevel);
                pfValue.OrganizationLevel = request.OrganizationLevel; //Enum.GetName(typeof(OrganizationLevelEnum), request.OrganizationLevel);
                await professionalExperienceRepository.UpdateAsync(pfValue).ConfigureAwait(false);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<EditProfessionalExperienceCommand>
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
                //RuleFor(x => x.PositionLevel).NotEmpty();
                //RuleFor(x => x.OrganizationLevel).NotEmpty();
            }
        }
        #endregion
    }
}
