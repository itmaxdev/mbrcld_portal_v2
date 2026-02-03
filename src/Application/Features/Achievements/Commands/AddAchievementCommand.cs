using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Achievements.Commands
{
    public sealed class AddAchievementCommand : IRequest<Result<Guid>>
    {
        #region Command
        public string Description { get; set; }
        public string Description_AR { get; set; }
        public string SummaryOfAchievement { get; set; }
        public string SummaryOfAchievement_AR { get; set; }
        public int? PopulationImpact { get; set; }
        public int? FinancialImpact { get; set; }
        public string Organization { get; set; }
        public string Organization_AR { get; set; }
        public string YearOfAchievement { get; set; }
        public Guid ContactId { get; set; }
        public int? AchievementImpact { get; set; }
        public bool Completed { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddAchievementCommand, Result<Guid>>
        {
            private readonly IAchievementRepository AchievementRepository;

            public CommandHandler(IAchievementRepository AchievementRepository)
            {
                this.AchievementRepository = AchievementRepository;
            }

            public async Task<Result<Guid>> Handle(AddAchievementCommand request, CancellationToken cancellationToken)
            {
                var achievement = Achievement.Create(
                    description: request.Description,
                    description_Ar: request.Description_AR,
                    summaryOfAchievement: request.SummaryOfAchievement,
                    summaryOfAchievement_AR: request.SummaryOfAchievement_AR,
                    populationImpactList: request.PopulationImpact,
                    financialImpact: request.FinancialImpact,
                    organization: request.Organization,
                    organization_AR: request.Organization_AR,
                    yearOfAchievement: request.YearOfAchievement,
                    contactId: request.ContactId,
                    achievementImpact: request.AchievementImpact
                );

                await AchievementRepository.CreateAsync(achievement).ConfigureAwait(false);

                return achievement.Id;
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddAchievementCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Description).NotNull().NotEmpty();
                RuleFor(x => x.Description_AR).NotNull().NotEmpty();
                RuleFor(x => x.SummaryOfAchievement).NotNull().NotEmpty();
                RuleFor(x => x.SummaryOfAchievement_AR).NotNull().NotEmpty();
                RuleFor(x => x.Organization).NotNull().NotEmpty();
                RuleFor(x => x.Organization_AR).NotNull().NotEmpty();
                RuleFor(x => x.YearOfAchievement).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
