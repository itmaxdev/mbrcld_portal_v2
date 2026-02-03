using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListTeamMembersByModuleIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UniversityId { get; set; }
        public string UniversityName { get; set; }
        public string JobPosition { get; set; }
        public string PictureUrl { get; set; }
        public string LinkedIn { get; set; }
        public string Email { get; set; }
        public string AboutMember { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<UniversityTeamMember, ListTeamMembersByModuleIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.FirstName + " " +src.LastName))
                    .ForMember(dst => dst.UniversityId, x => x.MapFrom(src => src.UniversityId))
                    .ForMember(dst => dst.UniversityName, x => x.MapFrom(src => src.UniversityName))
                    .ForMember(dst => dst.LinkedIn, x => x.MapFrom(src => src.LinkedIn))
                    .ForMember(dst => dst.AboutMember, x => x.MapFrom(src => src.AboutMember))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.Email))
                    .ForMember(dst => dst.JobPosition, x => x.MapFrom(src => src.JobPosition));
            }
        }
        #endregion
    }
}
