using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.TrainingCourses.Queries
{
    public sealed class ListUserTrainingCoursesViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Provider { get; set; }
        public string Country { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<TrainingCourse, ListUserTrainingCoursesViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Provider, x => x.MapFrom(src => src.Provider))
                    .ForMember(dst => dst.GraduationDate, x => x.MapFrom(src => src.GraduationDate))
                    .ForMember(dst => dst.Country, x => x.MapFrom(src => src.Country == null ? null : src.Country.IsoCode));
            }
        }
        #endregion
    }
}
