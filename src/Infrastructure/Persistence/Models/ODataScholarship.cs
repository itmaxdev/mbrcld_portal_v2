using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_scholarship")]
    internal sealed class ODataScholarship
    {

        [DataMember(Name = "do_scholarshipid")]
        internal Guid ScholarshipId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_fromdate")]
        internal DateTime FromDate { get; set; }

        [DataMember(Name = "do_todate")]
        internal DateTime ToDate { get; set; }

        [DataMember(Name = "do_description")]
        internal string Description { get; set; }

        [DataMember(Name = "do_openforregistration ")]
        public bool OpenForRegistration { get; set; }

        [DataMember(Name = "statuscode ")]
        public string StatusCode { get; set; }

        [DataMember(Name = "createdon")]
        public DateTime CreatedOn { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataScholarship, Scholarship>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.ScholarshipId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.FromDate, x => x.MapFrom(src => src.FromDate))
                    .ForMember(dst => dst.ToDate, x => x.MapFrom(src => src.ToDate))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.OpenForRegistration, x => x.MapFrom(src => src.OpenForRegistration))
                    .ForMember(dst => dst.StatusCode, x => x.MapFrom(src => src.StatusCode))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
