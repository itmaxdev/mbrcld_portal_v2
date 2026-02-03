namespace Mbrcld.Infrastructure.Persistence.Models
{
    using AutoMapper;
    using Mbrcld.Domain.Entities;
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "do_achievement")]
    public class ODataAchievement
    {
        [DataMember(Name = "do_achievementid")]
        internal Guid AchievementId { get; set; }

        [DataMember(Name = "do_description")]
        public string Description { get; set; }

        [DataMember(Name = "do_description_ar")]
        public string Description_AR { get; set; }

        [DataMember(Name = "do_summaryofachievement")]
        public string SummaryOfAchievement { get; set; }

        [DataMember(Name = "do_summaryofachievement_ar")]
        public string SummaryOfAchievement_AR { get; set; }

        [DataMember(Name = "do_populationimpactlist")]
        public int? PopulationImpactList { get; set; }       

        [DataMember(Name = "do_organisationprojectsituation")]
        public string Organization { get; set; }

        [DataMember(Name = "do_organisationprojectsituation_ar")]
        public string Organization_AR { get; set; }

        [DataMember(Name = "do_yearofachievement")]
        public string YearOfAchievement { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact ContactId { get; set; }

        [DataMember(Name = "do_financialimpact")]
        public int? FinancialImpact { get; set; }

        [DataMember(Name = "do_achievementimpact")]
        public int? AchievementImpact { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataAchievement, Achievement>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.AchievementId))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.Description_AR, x => x.MapFrom(src => src.Description_AR))
                    .ForMember(dst => dst.SummaryOfAchievement, x => x.MapFrom(src => src.SummaryOfAchievement))
                    .ForMember(dst => dst.SummaryOfAchievement_AR, x => x.MapFrom(src => src.SummaryOfAchievement_AR))
                    .ForMember(dst => dst.PopulationImpactList, x => x.MapFrom(src => src.PopulationImpactList))
                    .ForMember(dst => dst.Organization, x => x.MapFrom(src => src.Organization))
                    .ForMember(dst => dst.Organization_AR, x => x.MapFrom(src => src.Organization_AR))
                    .ForMember(dst => dst.YearOfAchievement, x => x.MapFrom(src => src.YearOfAchievement))
                    .ForMember(dst => dst.ContactId, x => x.MapFrom(src => src.ContactId.ContactId))
                    .ForMember(dst => dst.FinancialImpact, x => x.MapFrom(src => src.FinancialImpact))
                    .ForMember(dst => dst.AchievementImpact, x => x.MapFrom(src => src.AchievementImpact))

                    .ReverseMap();
            }
        }
        #endregion
    }
}

