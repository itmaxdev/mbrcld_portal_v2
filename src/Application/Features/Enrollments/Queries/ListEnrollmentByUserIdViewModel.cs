using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Enrollments.Queries
{
    public sealed class ListEnrollmentByUserIdViewModel
    {
        public Guid Id { get; set; }       
        public string Status { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Enrollment, ListEnrollmentByUserIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                //.ForMember(dst => dst.Status, x => x.MapFrom(src => src.Stage? "applied" : "enrolled"));
                .ForMember(dst => dst.Status, x => x.MapFrom(src => src.Stage == 0 ? "applied" : "enrolled"));
            }
        }
        #endregion
    }
}
