using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetAlumniEliteClubViewModel
    {
        public bool IsActiveMember { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EliteClub, GetAlumniEliteClubViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
