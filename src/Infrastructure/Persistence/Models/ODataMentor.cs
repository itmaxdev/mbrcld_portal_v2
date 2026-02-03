using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_mentor")]
    internal sealed class ODataMentor
    {
        [DataMember(Name = "do_mentorid")]
        internal Guid MentorId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_email")]
        internal string Email { get; set; }

        [DataMember(Name = "do_aboutmentor")]
        internal string AboutMentor { get; set; }

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

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataMentor, Mentor>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.MentorId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.Email))
                    .ForMember(dst => dst.AboutMentor, x => x.MapFrom(src => src.AboutMentor))
                    .ForMember(dst => dst.Education, x => x.MapFrom(src => src.Education))
                    .ForMember(dst => dst.Nationality, x => x.MapFrom(src => src.Nationality))
                    .ForMember(dst => dst.ResidenceCountry, x => x.MapFrom(src => src.ResidenceCountry))
                    .ForMember(dst => dst.JobPosition, x => x.MapFrom(src => src.JobPosition))
                    .ForMember(dst => dst.LinkedIn, x => x.MapFrom(src => src.LinkedIn))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
