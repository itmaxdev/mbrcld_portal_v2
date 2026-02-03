using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_educationqualifications")]
    internal sealed class ODataEducationQualification
    {
        [DataMember(Name = "do_educationqualificationid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }

        [DataMember(Name = "do_university")]
        internal string University { get; set; }

        [DataMember(Name = "do_university_ar")]
        internal string University_Ar { get; set; }

        [DataMember(Name = "do_graduationdate")]
        internal DateTime Graduationdate { get; set; }

        [DataMember(Name = "do_specialization")]
        internal string Specialization { get; set; }

        [DataMember(Name = "do_specialization_ar")]
        internal string Specialization_Ar { get; set; }

        [DataMember(Name = "do_degree")]
        internal int Degree { get; set; }

        [DataMember(Name = "do_CountryId")]
        internal ODataCountry Country { get; set; }
        [DataMember(Name = "do_city")]
        internal string City { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEducationQualification, EducationQualification>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Contact, x => x.MapFrom(src => src.Contact.ContactId))
                    .ForMember(dst => dst.University, x => x.MapFrom(src => src.University))
                    .ForMember(dst => dst.University_Ar, x => x.MapFrom(src => src.University_Ar))
                    .ForMember(dst => dst.Graduationdate, x => x.MapFrom(src => src.Graduationdate))
                    .ForMember(dst => dst.Specialization, x => x.MapFrom(src => src.Specialization))
                    .ForMember(dst => dst.Specialization_Ar, x => x.MapFrom(src => src.Specialization_Ar))
                    .ForMember(dst => dst.Degree, x => x.MapFrom(src => src.Degree))
                    .ForMember(dst => dst.Country, x => x.MapFrom(src => src.Country))
                    .ForMember(dst => dst.City, x => x.MapFrom(src => src.City))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
