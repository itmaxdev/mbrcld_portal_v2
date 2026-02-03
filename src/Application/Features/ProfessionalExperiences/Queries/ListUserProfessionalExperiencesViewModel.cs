using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.ProfessionalExperiences.Queries
{
    public sealed class ListUserProfessionalExperiencesViewModel
    {
        public Guid Id { get; set; }
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

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ProfessionalExperience, ListUserProfessionalExperiencesViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.JobTitle, x => x.MapFrom(src => src.JobTitle))
                    .ForMember(dst => dst.JobTitle_AR, x => x.MapFrom(src => src.JobTitle_Ar))
                    .ForMember(dst => dst.OrganizationName, x => x.MapFrom(src => src.OrganizationName))
                    .ForMember(dst => dst.OrganizationName_AR, x => x.MapFrom(src => src.OrganizationName_Ar))
                    .ForMember(dst => dst.From, x => x.MapFrom(src => src.From))
                    .ForMember(dst => dst.To, x => x.MapFrom(src => src.To))
                    .ForMember(dst => dst.Industry, x => x.MapFrom(src => src.Industry))
                    .ForMember(dst => dst.Sector, x => x.MapFrom(src => src.Sector))
                    .ForMember(dst => dst.OtherIndustry, x => x.MapFrom(src => src.OtherIndustry))
                    .ForMember(dst => dst.OtherSector, x => x.MapFrom(src => src.OtherSector))
                    .ForMember(dst => dst.OrganizationSize, x => x.MapFrom(src => src.OrganizationSize))
                    .ForMember(dst => dst.PositionLevel, x => x.MapFrom(src => src.PositionLevel))
                    .ForMember(dst => dst.OrganizationLevel, x => x.MapFrom(src => src.OrganizationLevel))
                    .ForMember(dst => dst.Completed, x => x.MapFrom(src => src.Completed));

            }
        }
        #endregion
    }
}
