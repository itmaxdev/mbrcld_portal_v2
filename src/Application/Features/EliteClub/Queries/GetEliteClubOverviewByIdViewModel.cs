using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetEliteClubOverviewByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
       
        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EliteClub, GetEliteClubOverviewByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Overview, x => x.MapFrom(src => src.Overview))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
