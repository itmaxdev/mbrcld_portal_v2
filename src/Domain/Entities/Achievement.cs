namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class Achievement : EntityBase
    {
        public string Description { get; set; }
        public string Description_AR { get; set; }
        public string SummaryOfAchievement { get; set; }
        public string SummaryOfAchievement_AR { get; set; }
        public int? PopulationImpactList { get; set; }
        public string Organization { get; set; }
        public string Organization_AR { get; set; }
        public string YearOfAchievement { get; set; }
        public Guid ContactId { get; set; }
        public int? FinancialImpact { get; set; }
        public int? AchievementImpact { get; set; }
        public bool Completed { get; set; }
        private Achievement()
        { }

        public static Achievement Create(
           string description,
           string description_Ar,
           string summaryOfAchievement,
           string summaryOfAchievement_AR,
           int? populationImpactList,
           int? financialImpact,
           string organization,
           string organization_AR,
           string yearOfAchievement,
           Guid contactId,
           int? achievementImpact
           )
        {
            Guard.Argument(description, nameof(description)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(description_Ar, nameof(description_Ar)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(summaryOfAchievement, nameof(summaryOfAchievement)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(summaryOfAchievement_AR, nameof(summaryOfAchievement_AR)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(populationImpactList, nameof(populationImpactList)).NotEqual(default);
            Guard.Argument(organization, nameof(organization)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(organization_AR, nameof(organization_AR)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(yearOfAchievement, nameof(yearOfAchievement)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(financialImpact, nameof(financialImpact)).NotEqual(default);
            Guard.Argument(achievementImpact, nameof(achievementImpact)).NotEqual(default);
            Guard.Argument(contactId, nameof(contactId)).NotEqual(default);

            var achievement = new Achievement();
            achievement.Id = Guid.NewGuid();
            achievement.Description = description;
            achievement.Description_AR = description_Ar;
            achievement.SummaryOfAchievement = summaryOfAchievement;
            achievement.SummaryOfAchievement_AR = summaryOfAchievement_AR;
            achievement.PopulationImpactList = populationImpactList;
            achievement.Organization = organization;
            achievement.Organization_AR = organization_AR;
            achievement.YearOfAchievement = yearOfAchievement;
            achievement.ContactId = contactId;
            achievement.FinancialImpact = financialImpact;
            achievement.AchievementImpact = achievementImpact;

            return achievement;
        }
    }
}