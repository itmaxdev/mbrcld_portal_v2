using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class PinnedPostsViewModel
    {
        public string VideoUrl { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Post, PinnedPostsViewModel>()
                    .ForMember(dst => dst.VideoUrl, x => x.MapFrom(src => src.VideoUrl));
            }
        }
        #endregion


    }
}
