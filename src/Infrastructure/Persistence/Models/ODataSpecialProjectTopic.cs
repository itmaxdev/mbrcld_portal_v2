using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_specialprojecttopic")]
    internal sealed class ODataSpecialProjectTopic
    {
        [DataMember(Name = "do_specialprojecttopicid")]
        internal Guid SpecialProjectTopicId { get; set; }

        [DataMember(Name = "do_name")]
        internal string SpecialProjectTopicName { get; set; }

        [DataMember(Name = "statuscode")]
        internal int status { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataSpecialProjectTopic, SpecialProjectTopic>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.SpecialProjectTopicId))
                    .ForMember(dst => dst.SpecialProjectTopicName, x => x.MapFrom(src => src.SpecialProjectTopicName))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
