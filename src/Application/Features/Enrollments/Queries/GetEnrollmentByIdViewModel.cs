using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Enrollments.Queries
{
    public sealed class GetEnrollmentByIdViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string PymetricsUrl { get; set; }
        public bool? PymetricsCompleted { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Enrollment, GetEnrollmentByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    //.ForMember(dst => dst.Status, x => x.MapFrom(src => src.Stage ? "applied" : "enrolled"))
                    .ForMember(dst => dst.Status, x => x.MapFrom(src => src.Stage == 0 ? "applied" : "enrolled"))
                    .ForMember(dst => dst.PymetricsUrl, x => x.MapFrom(src => src.PymetricsUrl))
                    .ForMember(dst => dst.PymetricsCompleted, x => x.MapFrom(src => src.PymetricsStatus == 4 || src.PymetricsStatus == 5));
            }
        }
        #endregion
    }
}
