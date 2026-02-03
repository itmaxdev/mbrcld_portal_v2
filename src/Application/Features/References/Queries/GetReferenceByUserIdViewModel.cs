using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.References.Queries
{
    public sealed class ListUserReferencesViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string FullName_AR { get; set; }
        public string JobTitle { get; set; }
        public string JobTitle_AR { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationName_AR { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsCompleted { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Reference, ListUserReferencesViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.FullName, x => x.MapFrom(src => src.FullName))
                    .ForMember(dst => dst.FullName_AR, x => x.MapFrom(src => src.FullName_Ar))
                    .ForMember(dst => dst.JobTitle, x => x.MapFrom(src => src.JobTitle))
                     .ForMember(dst => dst.JobTitle_AR, x => x.MapFrom(src => src.JobTitle_Ar))
                    .ForMember(dst => dst.OrganizationName, x => x.MapFrom(src => src.OrganizationName))
                    .ForMember(dst => dst.OrganizationName_AR, x => x.MapFrom(src => src.OrganizationName_Ar))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.Email))
                    .ForMember(dst => dst.Mobile, x => x.MapFrom(src => src.Mobile))
                    .ForMember(dst => dst.IsCompleted, x => x.MapFrom(src => src.IsCompleted));
            }
        }
        #endregion
    }
}