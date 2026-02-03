using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetEliteMembershipAttendanceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Description { get; set; }
        public int AttendanceStatus { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EliteMembershipAttendance, GetEliteMembershipAttendanceViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.FromDate, x => x.MapFrom(src => src.FromDate))
                    .ForMember(dst => dst.ToDate, x => x.MapFrom(src => src.ToDate))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.AttendanceStatus, x => x.MapFrom(src => src.AttendanceStatus))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
