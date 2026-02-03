using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Memberships.Queries
{
    public sealed class ListUserMembershipsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionName_AR { get; set; }
        public DateTime JoinDate { get; set; }
        public string RoleName { get; set; }
        public string RoleName_AR { get; set; }
        public bool Active { get; set; }
        public int? MembershipLevel { get; set; }
        public bool Completed { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Membership, ListUserMembershipsViewModel>()
                    .ForMember(dst => dst.JoinDate, x => x.MapFrom(src => src.JoinDate))
                    .ForMember(dst => dst.Active, x => x.MapFrom(src => src.Active))
                    .ForMember(dst => dst.MembershipLevel, x => x.MapFrom(src => src.MembershipLevel))
                    .ForMember(dst => dst.Completed, x => x.MapFrom(src => src.Completed));

            }
        }
        #endregion
    }
}
