using AutoMapper;
using Mbrcld.Domain.Entities;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListIndustriesViewModel
    {
        public string Id { get; set; }
        public string Label { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Industry, ListIndustriesViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Label, x => x.MapFrom(src => src.Label));
            }
        }
        #endregion
    }
}
