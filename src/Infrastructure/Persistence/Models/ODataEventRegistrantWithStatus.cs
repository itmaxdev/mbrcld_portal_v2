using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_eventregistrant")]
    internal sealed class ODataEventRegistrantWithStatus
    {
        [DataMember(Name = "do_eventregistrantid")]
        internal Guid EventRegistrantId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact ContactId { get; set; }

        [DataMember(Name = "do_EventId")]
        internal ODataEvent EventId { get; set; }

        [DataMember(Name = "statuscode")]
        internal int? StatusCode { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEventRegistrantWithStatus, EventRegistrant>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.EventRegistrantId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.EventId.Name))
                    .ForMember(dst => dst.UserId, x => x.MapFrom(src => src.ContactId.ContactId))
                    .ForMember(dst => dst.EventId, x => x.MapFrom(src => src.EventId.EventId))
                    .ForMember(dst => dst.StatusCode, x => x.MapFrom(src => src.StatusCode))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
