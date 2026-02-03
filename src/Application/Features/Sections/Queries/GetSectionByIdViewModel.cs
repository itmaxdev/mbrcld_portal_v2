using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetSectionByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public int Duration { get; set; }
        public decimal Order { get; set; }
        public Guid MaterialId { get; set; }
        public int? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public int SectionStatus { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Section, GetSectionByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Name_AR, x => x.MapFrom(src => src.Name_AR))
                    .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                    .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order))
                    .ForMember(dst => dst.MaterialId, x => x.MapFrom(src => src.MaterialId))
                    .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                    .ForMember(dst => dst.Status, x => x.MapFrom(src => src.Status))
                    .ForMember(dst => dst.SectionStatus, x => x.MapFrom(src => src.SectionStatus))
                    .ForMember(dst => dst.PublishDate, x => x.MapFrom(src => src.PublishDate));
            }
        }
        #endregion
    }
}
