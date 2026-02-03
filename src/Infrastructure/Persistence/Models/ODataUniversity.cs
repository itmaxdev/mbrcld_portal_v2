using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_university")]
    internal sealed class ODataUniversity
    {
        [DataMember(Name = "do_universityid")]
        internal Guid UniversityId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_phone")]
        internal string Phone { get; set; }

        [DataMember(Name = "do_email")]
        internal string Email { get; set; }

        [DataMember(Name = "do_aboutuniversity")]
        internal string AboutUniversity { get; set; }

        [DataMember(Name = "do_AccessInstructorId")]
        internal ODataContact AccessInstructor { get; set; }

        [DataMember(Name = "do_city")]
        internal string City { get; set; }

        [DataMember(Name = "do_CountryId")]
        internal ODataCountry Country { get; set; }

        [DataMember(Name = "do_address")]
        internal string Address { get; set; }

        [DataMember(Name = "do_instagram")]
        internal string Instagram { get; set; }

        [DataMember(Name = "do_linkedin")]
        internal string LinkedIn { get; set; }

        [DataMember(Name = "do_pobox")]
        internal string POBox { get; set; }

        [DataMember(Name = "do_twitter")]
        internal string Twitter { get; set; }

        [DataMember(Name = "do_website")]
        internal string Website { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataUniversity, University>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.UniversityId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Phone, x => x.MapFrom(src => src.Phone))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.Email))
                    .ForMember(dst => dst.AboutUniversity, x => x.MapFrom(src => src.AboutUniversity))
                    .ForMember(dst => dst.AccessInstructorId, x => x.MapFrom(src => src.AccessInstructor.ContactId))
                    .ForMember(dst => dst.AccessInstructorName, x => x.MapFrom(src => src.AccessInstructor.FirstName + " " + src.AccessInstructor.MiddleName + " " + src.AccessInstructor.LastName))
                    .ForMember(dst => dst.City, x => x.MapFrom(src => src.City))
                    //.ForMember(dst => dst.CityName, x => x.MapFrom(src => src.CityId))
                    .ForMember(dst => dst.Country, x => x.MapFrom(src => src.Country))
                    .ForMember(dst => dst.Address, x => x.MapFrom(src => src.Address))
                    .ForMember(dst => dst.Instagram, x => x.MapFrom(src => src.Instagram))
                    .ForMember(dst => dst.LinkedIn, x => x.MapFrom(src => src.LinkedIn))
                    .ForMember(dst => dst.POBox, x => x.MapFrom(src => src.POBox))
                    .ForMember(dst => dst.Twitter, x => x.MapFrom(src => src.Twitter))
                    .ForMember(dst => dst.Website, x => x.MapFrom(src => src.Website))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
