using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_elitemembershipattendance")]
    internal sealed class ODataEliteMembershipAttendance
    {
        [DataMember(Name = "do_elitemembershipattendanceid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_attendancestatus")]
        internal int AttendanceStatus { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }

        [DataMember(Name = "do_EliteMembershipId")]
        internal ODataEliteMembership EliteMembership { get; set; }


        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEliteMembershipAttendance, EliteMembershipAttendance>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.AttendanceStatus, x => x.MapFrom(src => src.AttendanceStatus))
                  .ForMember(dst => dst.ContactId, x => x.MapFrom(src => src.Contact.ContactId))
                  .ForMember(dst => dst.EliteMembeshipId, x => x.MapFrom(src => src.EliteMembership.Id))
                  .ForMember(dst => dst.FromDate, x => x.MapFrom(src => src.EliteMembership.FromDate))
                  .ForMember(dst => dst.ToDate, x => x.MapFrom(src => src.EliteMembership.ToDate))
                  .ForMember(dst => dst.Description, x => x.MapFrom(src => src.EliteMembership.Decription))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
