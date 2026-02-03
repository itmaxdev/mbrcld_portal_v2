using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.EducationQualifications.Queries
{
    public sealed class ListUserEducationQualificationsViewModel
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string University { get; set; }
        public string University_AR { get; set; }
        public string Specialization { get; set; }
        public string Specialization_AR { get; set; }
        public int Degree { get; set; }
        public string Country { get; set; }
        public DateTime GraduationDate { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EducationQualification, ListUserEducationQualificationsViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.University, x => x.MapFrom(src => src.University))
                    .ForMember(dst => dst.University_AR, x => x.MapFrom(src => src.University_Ar))
                    .ForMember(dst => dst.Specialization, x => x.MapFrom(src => src.Specialization))
                    .ForMember(dst => dst.Specialization_AR, x => x.MapFrom(src => src.Specialization_Ar))
                    .ForMember(dst => dst.GraduationDate, x => x.MapFrom(src => src.Graduationdate))
                    .ForMember(dst => dst.Degree, x => x.MapFrom(src => src.Degree))
                     .ForMember(dst => dst.City, x => x.MapFrom(src => src.City))
                    .ForMember(dst => dst.Country, x => x.MapFrom(src => src.Country == null ? null : src.Country.IsoCode));
            }
        }
        #endregion
    }
}
