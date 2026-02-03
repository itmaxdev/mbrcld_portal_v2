namespace Mbrcld.Infrastructure.Persistence.Models
{
    using AutoMapper;
    using Mbrcld.Domain.Entities;
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "do_membership")]
    public class ODataMembership
    {
        [DataMember(Name = "do_membershipid")]
        internal Guid MembershipId { get; set; }

        [DataMember(Name = "do_joindate")]
        internal DateTime JoinDate { get; set; }

        [DataMember(Name = "do_role")]
        internal string Role { get; set; }

        [DataMember(Name = "do_role_ar")]
        internal string Role_AR { get; set; }

        [DataMember(Name = "do_institutionname")]
        internal string InstitutionName { get; set; }

        [DataMember(Name = "do_institutionname_ar")]
        internal string InstitutionName_AR { get; set; }

        [DataMember(Name = "do_active")]
        internal bool Active { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact ContactId { get; set; }

        [DataMember(Name = "do_membershiplevel")]
        internal int? MembershipLevel { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataMembership, Membership>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.MembershipId))
                    .ForMember(dst => dst.RoleName, x => x.MapFrom(src => src.Role))
                    .ForMember(dst => dst.RoleName_AR, x => x.MapFrom(src => src.Role_AR))
                    .ForMember(dst => dst.JoinDate, x => x.MapFrom(src => src.JoinDate))
                    .ForMember(dst => dst.InstitutionName, x => x.MapFrom(src => src.InstitutionName))
                    .ForMember(dst => dst.InstitutionName_AR, x => x.MapFrom(src => src.InstitutionName_AR))
                    .ForMember(dst => dst.Active, x => x.MapFrom(src => src.Active))
                    .ForMember(dst => dst.UserId, x => x.MapFrom(src => src.ContactId))
                    .ForMember(dst => dst.MembershipLevel, x => x.MapFrom(src => src.MembershipLevel))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
