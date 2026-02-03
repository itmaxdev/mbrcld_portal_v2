using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_eventregistrant")]
    internal sealed class ODataEventRegistrant
    {
        [DataMember(Name = "do_eventregistrantid")]
        internal Guid EventRegistrantId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact ContactId { get; set; }

        [DataMember(Name = "do_EventId")]
        internal ODataEvent EventId { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEventRegistrant, EventRegistrant>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.EventRegistrantId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.UserId, x => x.MapFrom(src => src.ContactId.ContactId))
                    .ForMember(dst => dst.EventId, x => x.MapFrom(src => src.EventId.EventId))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
