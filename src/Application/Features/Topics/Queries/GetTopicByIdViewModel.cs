using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetTopicByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxCount { get; set; }
        public Guid ProgramId { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Topic, GetTopicByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.TopicName))
                    .ForMember(dst => dst.MaxCount, x => x.MapFrom(src => src.MaxCount))
                    .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.ProgramId))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
