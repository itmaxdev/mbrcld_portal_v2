using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_professionalexperiences")]
    internal sealed class ODataProfessionalExperience
    {
        [DataMember(Name = "do_professionalexperienceid")]
        internal Guid Id { get; set; }
        
        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }
        
        [DataMember(Name = "do_jobtitle")]
        internal string JobTitle { get; set; }
        
        [DataMember(Name = "do_organizationname")]
        internal string OrganizationName { get; set; }
        
        [DataMember(Name = "do_from")]
        internal DateTime From { get; set; }
        
        [DataMember(Name = "do_to")]
        internal DateTime? To { get; set; }
        
        [DataMember(Name = "do_IndustryId")]
        internal ODataIndustry Industry { get; set; }
        
        [DataMember(Name = "do_SectorId")]
        internal ODataSector Sector { get; set; }

        [DataMember(Name = "do_otherindustry")]
        internal string OtherIndustry { get; set; }

        [DataMember(Name = "do_othersector")]
        internal string OtherSector { get; set; }

        [DataMember(Name = "do_jobtitle_ar")]
        internal string JobTitle_Ar { get; set; }
        
        [DataMember(Name = "do_organizationname_ar")]
        internal string OrganizationName_Ar { get; set; }

        [DataMember(Name = "do_organizationsize")]
        internal int? OrganizationSize { get; set; }

        [DataMember(Name = "do_positionlevel")]
        internal int? PositionLevel { get; set; }

        [DataMember(Name = "do_organizationlevel")]
        internal int? OrganizationLevel { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataProfessionalExperience, ProfessionalExperience>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Contact, x => x.MapFrom(src => src.Contact.ContactId))
                    .ForMember(dst => dst.JobTitle, x => x.MapFrom(src => src.JobTitle))
                    .ForMember(dst => dst.OrganizationName, x => x.MapFrom(src => src.OrganizationName))
                    .ForMember(dst => dst.From, x => x.MapFrom(src => src.From))
                    .ForMember(dst => dst.To, x => x.MapFrom(src => src.To))
                    .ForMember(dst => dst.Industry, x => x.MapFrom(src => src.Industry.IndustryId))
                    .ForMember(dst => dst.Sector, x => x.MapFrom(src => src.Sector.SectorId))
                    .ForMember(dst => dst.JobTitle_Ar, x => x.MapFrom(src => src.JobTitle_Ar))
                    .ForMember(dst => dst.OrganizationName_Ar, x => x.MapFrom(src => src.OrganizationName_Ar))
                    .ForMember(dst => dst.OtherIndustry, x => x.MapFrom(src => src.OtherIndustry))
                    .ForMember(dst => dst.OtherSector, x => x.MapFrom(src => src.OtherSector))
                    .ForMember(dst => dst.OrganizationSize, x => x.MapFrom(src => src.OrganizationSize))
                    .ForMember(dst => dst.PositionLevel, x => x.MapFrom(src => src.PositionLevel))
                    .ForMember(dst => dst.OrganizationLevel, x => x.MapFrom(src => src.OrganizationLevel))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
