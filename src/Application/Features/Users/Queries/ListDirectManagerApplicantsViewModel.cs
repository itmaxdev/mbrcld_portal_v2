using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListDirectManagerApplicantsViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<User, ListDirectManagerApplicantsViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.FullName, x => x.MapFrom(src => src.FirstName + " " + src.MiddleName + " " + src.LastName))
                    .ForMember(dst => dst.ProfilePictureUrl, x => x.MapFrom(src => src.ProfilePictureUniqueId));
            }
        }
        #endregion
    }
}
