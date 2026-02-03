using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_universityteammember")]
    internal sealed class ODataUniversityTeamMember
    {
        [DataMember(Name = "do_universityteammemberid")]
        internal Guid UniversityTeamMemberId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_firstname")]
        internal string FirstName { get; set; }

        [DataMember(Name = "do_lastname")]
        internal string LastName { get; set; }

        [DataMember(Name = "do_email")]
        internal string Email { get; set; }

        [DataMember(Name = "do_aboutmember")]
        internal string AboutMember { get; set; }

        [DataMember(Name = "do_UniversityId")]
        internal ODataUniversity University { get; set; }

        [DataMember(Name = "do_education")]
        internal string Education { get; set; }

        [DataMember(Name = "do_NationalityId")]
        internal ODataCountry Nationality { get; set; }

        [DataMember(Name = "do_ResidenceCountryId")]
        internal ODataCountry ResidenceCountry { get; set; }

        [DataMember(Name = "do_jobposition")]
        internal string JobPosition { get; set; }

        [DataMember(Name = "do_linkedin")]
        internal string LinkedIn { get; set; }

        [DataMember(Name = "statecode")]
        internal int Status { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataUniversityTeamMember, UniversityTeamMember>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.UniversityTeamMemberId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.FirstName, x => x.MapFrom(src => src.FirstName))
                    .ForMember(dst => dst.LastName, x => x.MapFrom(src => src.LastName))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.Email))
                    .ForMember(dst => dst.AboutMember, x => x.MapFrom(src => src.AboutMember))
                    .ForMember(dst => dst.UniversityId, x => x.MapFrom(src => src.University.UniversityId))
                    .ForMember(dst => dst.UniversityName, x => x.MapFrom(src => src.University.Name))
                    .ForMember(dst => dst.Education, x => x.MapFrom(src => src.Education))
                    .ForMember(dst => dst.Nationality, x => x.MapFrom(src => src.Nationality))
                    .ForMember(dst => dst.ResidenceCountry, x => x.MapFrom(src => src.ResidenceCountry))
                    .ForMember(dst => dst.JobPosition, x => x.MapFrom(src => src.JobPosition))
                    .ForMember(dst => dst.status, x => x.MapFrom(src => src.Status))
                    .ForMember(dst => dst.LinkedIn, x => x.MapFrom(src => src.LinkedIn))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
