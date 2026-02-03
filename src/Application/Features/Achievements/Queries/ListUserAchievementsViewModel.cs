using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Achievements.Queries
{
    public sealed class ListUserAchievementsViewModel
    {
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
        public int? AchievementImpact { get; set; }
        public bool Completed { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Achievement, ListUserAchievementsViewModel>()
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.Description_AR, x => x.MapFrom(src => src.Description_AR))
                    .ForMember(dst => dst.Description_AR, x => x.MapFrom(src => src.Description_AR))
                    .ForMember(dst => dst.SummaryOfAchievement, x => x.MapFrom(src => src.SummaryOfAchievement))
                    .ForMember(dst => dst.SummaryOfAchievement_AR, x => x.MapFrom(src => src.SummaryOfAchievement_AR))
                    .ForMember(dst => dst.PopulationImpact, x => x.MapFrom(src => src.PopulationImpactList))
                    .ForMember(dst => dst.FinancialImpact, x => x.MapFrom(src => src.FinancialImpact))
                    .ForMember(dst => dst.Description_AR, x => x.MapFrom(src => src.Description_AR))
                    .ForMember(dst => dst.Organization, x => x.MapFrom(src => src.Organization))
                    .ForMember(dst => dst.Organization_AR, x => x.MapFrom(src => src.Organization_AR))
                    .ForMember(dst => dst.YearOfAchievement, x => x.MapFrom(src => src.YearOfAchievement))
                    .ForMember(dst => dst.AchievementImpact, x => x.MapFrom(src => src.AchievementImpact))
                    .ForMember(dst => dst.Completed, x => x.MapFrom(src => src.Completed));


            }
        }
        #endregion
    }
}
