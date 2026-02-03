using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListNewsFeedsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
        public string DocumentUrl { get; set; }
        public decimal Order { get; set; }
        public DateTime? MeetingStartDate { get; set; }
        public Guid ModuleId { get; set; }
        public string ModuleName { get; set; }
        public Guid InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool Liked { get; set; }
        public int? Likes { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<NewsFeed, ListNewsFeedsViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                    .ForMember(dst => dst.Text, x => x.MapFrom(src => src.Text))
                    .ForMember(dst => dst.Type, x => x.MapFrom(src => src.Type))
                    .ForMember(dst => dst.Url, x => x.MapFrom(src => src.Url))
                    .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order))
                    .ForMember(dst => dst.MeetingStartDate, x => x.MapFrom(src => src.MeetingStartDate))
                    .ForMember(dst => dst.ModuleId, x => x.MapFrom(src => src.ModuleId))
                    .ForMember(dst => dst.ModuleName, x => x.MapFrom(src => src.ModuleName))
                    .ForMember(dst => dst.InstructorId, x => x.MapFrom(src => src.InstructorId))
                    .ForMember(dst => dst.InstructorName, x => x.MapFrom(src => src.InstructorName));
            }
        }
        #endregion
    }
}
