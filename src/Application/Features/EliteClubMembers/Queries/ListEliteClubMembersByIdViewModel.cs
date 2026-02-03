using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListEliteClubMembersByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EliteClubMember, ListEliteClubMembersByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.ContactId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.ContactFullName))
                    .ForMember(dst => dst.ProfilePictureUrl, x => x.MapFrom(src => src.PictureUrl));
            }
        }
        #endregion
    }
}
