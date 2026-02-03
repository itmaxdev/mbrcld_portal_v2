using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_eventanswer")]
    internal sealed class ODataEventAnswers
    {
   

        [DataMember(Name = "do_Answer")]
        internal string Answer { get; set; }

        [DataMember(Name = "do_EventId")]
        internal ODataEvent EventId { get; set; }


        [DataMember(Name = "do_EventAnswerId")]
        internal Guid EventAnswerId { get; set; }

        [DataMember(Name = "do_EventQuestionId")]
        internal ODataEventQuestion EventQuestionId { get; set; }

        [DataMember(Name = "do_eventregistrantid")]
        internal ODataEventRegistrant EventRegistrantId { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEventAnswers, EventAnswer>()
                    .ForMember(dst => dst.Answer, x => x.MapFrom(src => src.Answer))
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.EventAnswerId))
                    .ForMember(dst => dst.EventId, x => x.MapFrom(src => src.EventId.EventId))
                    .ForMember(dst => dst.EventQuestionId, x => x.MapFrom(src => src.EventQuestionId.Id))
                    .ForMember(dst => dst.EventRegistrantId, x => x.MapFrom(src => src.EventRegistrantId.EventRegistrantId))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
