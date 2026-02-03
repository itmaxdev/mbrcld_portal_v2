namespace Mbrcld.Infrastructure.Persistence.Models
{
    using AutoMapper;
    using Mbrcld.Domain.Entities;
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "do_trainingcourse")]
    public class ODataTrainingCourse
    {
        [DataMember(Name = "do_trainingcourseid")]
        internal Guid TrainingCourseId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_graduationdate")]
        internal DateTime GraduationDate { get; set; }

        [DataMember(Name = "do_provider")]
        internal string Provider { get; set; }

        [DataMember(Name = "do_CountryId")]
        internal ODataCountry CountryId { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact ContactId { get; set; }


        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataTrainingCourse, TrainingCourse>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.TrainingCourseId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.GraduationDate, x => x.MapFrom(src => src.GraduationDate))
                    .ForMember(dst => dst.Provider, x => x.MapFrom(src => src.Provider))
                    .ForMember(dst => dst.Country, x => x.MapFrom(src => src.CountryId))
                    .ForMember(dst => dst.UserId, x => x.MapFrom(src => src.ContactId))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
