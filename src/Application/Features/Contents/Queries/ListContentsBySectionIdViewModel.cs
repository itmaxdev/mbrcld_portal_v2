using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListContentsBySectionIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
        public string DocumentUrl { get; set; }
        public decimal Order { get; set; }
        public DateTime StartDate { get; set; }
        public Guid SectionId { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Content, ListContentsBySectionIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                    .ForMember(dst => dst.Text, x => x.MapFrom(src => src.Text))
                    .ForMember(dst => dst.Type, x => x.MapFrom(src => src.Type))
                    .ForMember(dst => dst.Url, x => x.MapFrom(src => src.Url))
                    .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                    .ForMember(dst => dst.SectionId, x => x.MapFrom(src => src.SectionId))
                    .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order));
            }
        }
        #endregion
    }
}
