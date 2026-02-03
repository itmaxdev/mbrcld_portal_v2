using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_event")]
    internal sealed class ODataEvent
    {
        [DataMember(Name = "do_eventid")]
        internal Guid EventId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_fromdate")]
        internal DateTime FromDate { get; set; }

        [DataMember(Name = "do_todate")]
        internal DateTime ToDate { get; set; }

        [DataMember(Name = "do_duration")]
        internal int Duration { get; set; }

        [DataMember(Name = "do_description")]
        internal string Description { get; set; }

        [DataMember(Name = "do_maxcapacity")]
        internal int MaxCapacity { get; set; }

        [DataMember(Name = "createdon")]
        public DateTime CreatedOn { get; set; }

        [DataMember(Name = "do_hasquestions")]
        public bool HasQuestions { get; set; }

        [DataMember(Name = "do_alumnionly")]
        public bool AlumniOnly { get; set; }
        

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEvent, Event>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.EventId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.FromDate, x => x.MapFrom(src => src.FromDate))
                    .ForMember(dst => dst.ToDate, x => x.MapFrom(src => src.ToDate))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                    .ForMember(dst => dst.MaxCapacity, x => x.MapFrom(src => src.MaxCapacity))
                    .ForMember(dst => dst.HasQuestions, x => x.MapFrom(src => src.HasQuestions))
                    .ForMember(dst => dst.AlumniOnly, x => x.MapFrom(src => src.AlumniOnly))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
