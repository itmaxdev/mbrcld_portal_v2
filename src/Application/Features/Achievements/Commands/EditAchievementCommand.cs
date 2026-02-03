using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Achievements.Commands
{
    public sealed class EditAchievementCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Description_AR { get; set; }
        public string SummaryOfAchievement { get; set; }
        public string SummaryOfAchievement_AR { get; set; }
        public int? PopulationImpact { get; set; }
        public int? FinancialImpact { get; set; }
        public string Organization { get; set; }
        public string Organization_AR { get; set; }
        public string YearOfAchievement { get; set; }
        public int? AchievementImpact { get; set;}
        public bool Completed { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditAchievementCommand, Result>
        {
            private readonly IAchievementRepository AchievementRepository;

            public CommandHandler(IAchievementRepository AchievementRepository)
            {
                this.AchievementRepository = AchievementRepository;
            }

            public async Task<Result> Handle(EditAchievementCommand request, CancellationToken cancellationToken)
            {
                var AchievementPull = await AchievementRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (AchievementPull.HasNoValue)
                {
                    throw new Exception();
                }

                var achievementValue = AchievementPull.Value;
                achievementValue.Description = request.Description;
                achievementValue.Description_AR = request.Description_AR;
                achievementValue.SummaryOfAchievement = request.SummaryOfAchievement;
                achievementValue.SummaryOfAchievement_AR = request.SummaryOfAchievement_AR;
                achievementValue.PopulationImpactList = request.PopulationImpact;
                achievementValue.FinancialImpact = request.FinancialImpact;
                achievementValue.Organization = request.Organization;
                achievementValue.Organization_AR = request.Organization_AR;
                achievementValue.YearOfAchievement = request.YearOfAchievement;
                achievementValue.AchievementImpact = request.AchievementImpact;

                await AchievementRepository.UpdateAsync(achievementValue).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<EditAchievementCommand>
        {
            public CommandValidator()
            {
                ////RuleFor(x => x.Id).NotNull().NotEmpty();
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
