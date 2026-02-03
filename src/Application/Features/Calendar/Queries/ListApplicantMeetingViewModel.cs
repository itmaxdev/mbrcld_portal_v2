using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListApplicantMeetingViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public Guid ContactID { get; set; }
        public Guid MeetingId { get; set; }
        public string MeetingUrl { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Calendar, ListApplicantMeetingViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                    .ForMember(dst => dst.EndDate, x => x.MapFrom(src => src.EndDate))
                    .ForMember(dst => dst.ContactID, x => x.MapFrom(src => src.ContactID))
                    .ForMember(dst => dst.MeetingId, x => x.MapFrom(src => src.MeetingId))
                    .ForMember(dst => dst.MeetingUrl, x => x.MapFrom(src => src.MeetingUrl))
                    .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration));
            }
        }
        #endregion
    }
}
