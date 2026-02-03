using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListTopicsByProgramIdViewModel
    {
        public Guid Id { get; set; }
        public string TopicName { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Topic, ListTopicsByProgramIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.TopicName, x => x.MapFrom(src => src.TopicName));
            }
        }
        #endregion
    }
}
