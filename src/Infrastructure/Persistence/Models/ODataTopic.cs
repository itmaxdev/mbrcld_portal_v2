using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_projecttopic")]
    internal sealed class ODataTopic
    {
        [DataMember(Name = "do_projecttopicid")]
        internal Guid TopicId { get; set; }

        [DataMember(Name = "do_name")]
        internal string TopicName { get; set; }

        [DataMember(Name = "do_maxcount")]
        internal int MaxCount { get; set; }

        [DataMember(Name = "do_programid")]
        public ODataProgram Program { get; set; }

        [DataMember(Name = "do_CohortId")]
        public ODataCohort Cohort { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataTopic, Topic>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.TopicId))
                    .ForMember(dst => dst.TopicName, x => x.MapFrom(src => src.TopicName))
                    .ForMember(dst => dst.MaxCount, x => x.MapFrom(src => src.MaxCount))
                    .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.Program.Id))
                    .ForMember(dst => dst.CohortId, x => x.MapFrom(src => src.Cohort.Id))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
