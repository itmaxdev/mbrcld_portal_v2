using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListEventsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public int Duration { get; set; }
        public string PictureUrl { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Event, ListEventsViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.FromDate, x => x.MapFrom(src => src.FromDate))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration));
            }
        }
        #endregion
    }
}
