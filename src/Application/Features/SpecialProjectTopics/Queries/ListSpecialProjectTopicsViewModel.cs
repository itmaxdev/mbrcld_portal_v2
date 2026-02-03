using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListSpecialProjectTopicsViewModel
    {
        public Guid SpecialProjectTopicId { get; set; }
        public string SpecialProjectTopicName { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<SpecialProjectTopic, ListSpecialProjectTopicsViewModel>()
                    .ForMember(dst => dst.SpecialProjectTopicId, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.SpecialProjectTopicName, x => x.MapFrom(src => src.SpecialProjectTopicName));
            }
        }
        #endregion
    }
}
