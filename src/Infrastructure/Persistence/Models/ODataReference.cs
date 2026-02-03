using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_references")]
    internal sealed class ODataReference
    {
        [DataMember(Name = "do_referenceid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_ReferringId")]
        internal ODataContact Contact { get; set; }

        [DataMember(Name = "do_name")]
        internal string FullName { get; set; }

        [DataMember(Name = "do_fullname_ar")]
        internal string FullName_Ar { get; set; }

        [DataMember(Name = "do_jobtitle")]
        internal string JobTitle { get; set; }

        [DataMember(Name = "do_jobtitle_ar")]
        internal string JobTitle_Ar { get; set; }

        [DataMember(Name = "do_organization")]
        internal string OrganizationName { get; set; }

        [DataMember(Name = "do_organization_ar")]
        internal string OrganizationName_Ar { get; set; }

        [DataMember(Name = "do_email")]
        internal string Email { get; set; }

        [DataMember(Name = "do_mobile")]
        internal string Mobile { get; set; }

        [DataMember(Name = "do_iscompleted")]
        internal bool IsCompleted { get; set; }

        [DataMember(Name = "do_strengths")]
        internal string Strengths { get; set; }

        [DataMember(Name = "do_impact")]
        internal string Impact { get; set; }

        [DataMember(Name = "do_competency")]
        internal string Competency { get; set; }

        [DataMember(Name = "do_capacity")]
        internal string Capacity { get; set; }

        [DataMember(Name = "do_areasofimprovement")]
        internal string AreasOfImprovement { get; set; }

        [DataMember(Name = "do_additionaldetails")]
        internal string AdditionalDetails { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataReference, Reference>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Contact, x => x.MapFrom(src => src.Contact.ContactId))
                    .ForMember(dst => dst.FullName, x => x.MapFrom(src => src.FullName))
                    .ForMember(dst => dst.FullName_Ar, x => x.MapFrom(src => src.FullName_Ar))
                    .ForMember(dst => dst.JobTitle, x => x.MapFrom(src => src.JobTitle))
                    .ForMember(dst => dst.JobTitle_Ar, x => x.MapFrom(src => src.JobTitle_Ar))
                    .ForMember(dst => dst.OrganizationName, x => x.MapFrom(src => src.OrganizationName))
                    .ForMember(dst => dst.OrganizationName_Ar, x => x.MapFrom(src => src.OrganizationName_Ar))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.Email))
                    .ForMember(dst => dst.Mobile, x => x.MapFrom(src => src.Mobile))
                    .ForMember(dst => dst.IsCompleted, x => x.MapFrom(src => src.IsCompleted))
                    .ForMember(dst => dst.Strengths, x => x.MapFrom(src => src.Strengths))
                    .ForMember(dst => dst.Impact, x => x.MapFrom(src => src.Impact))
                    .ForMember(dst => dst.Competency, x => x.MapFrom(src => src.Competency))
                    .ForMember(dst => dst.Capacity, x => x.MapFrom(src => src.Capacity))
                    .ForMember(dst => dst.AreasOfImprovement, x => x.MapFrom(src => src.AreasOfImprovement))
                    .ForMember(dst => dst.AdditionalDetails, x => x.MapFrom(src => src.AdditionalDetails))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
