using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_scholarshipregistration")]
    internal sealed class ODataScholarshipRegistration
    {

        [DataMember(Name = "do_scholarshipregistrationid")]
        internal Guid ScholarshipRegistrationId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact ContactId { get; set; }

        [DataMember(Name = "do_ScholarshipId")]
        internal ODataScholarship ScholarshipId { get; set; }

        [DataMember(Name = "statuscode")]
        internal int? StatusCode { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataScholarshipRegistration, ScholarshipRegistration>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.ScholarshipRegistrationId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.ScholarshipId.Name))
                    .ForMember(dst => dst.UserId, x => x.MapFrom(src => src.ContactId.ContactId))
                    .ForMember(dst => dst.ScholarshipId, x => x.MapFrom(src => src.ScholarshipId.ScholarshipId))
                    .ForMember(dst => dst.StatusCode, x => x.MapFrom(src => src.StatusCode))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
