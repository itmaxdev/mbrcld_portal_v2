using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetMaterialByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public string Location { get; set; }
        public int Duration { get; set; }
        public decimal Order { get; set; }
        public Guid ModuleId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public int Status { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Material, GetMaterialByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Name_AR, x => x.MapFrom(src => src.Name_AR))
                    .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                    .ForMember(dst => dst.Location, x => x.MapFrom(src => src.Location))
                    .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order))
                    .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                    .ForMember(dst => dst.ModuleId, x => x.MapFrom(src => src.ModuleId))
                    .ForMember(dst => dst.Status, x => x.MapFrom(src => src.Status))
                    .ForMember(dst => dst.PublishDate, x => x.MapFrom(src => src.PublishDate));
            }
        }
        #endregion
    }
}
