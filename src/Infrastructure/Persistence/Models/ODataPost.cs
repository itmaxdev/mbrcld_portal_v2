using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_post")] // To Be Edited
    internal sealed class ODataPost
    {
        [DataMember(Name = "do_postid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_description")]
        internal string Desription { get; set; }

        [DataMember(Name = "do_poststatus")]
        internal int? PostStatus { get; set; }

        [DataMember(Name = "do_postdate")]
        internal DateTime? PostDate { get; set; }

        [DataMember(Name = "do_expirydate")]
        internal DateTime? ExpiryDate { get; set; }

        [DataMember(Name = "do_type")]
        internal int PostType { get; set; }

        [DataMember(Name = "do_videourl")]
        internal string VideoUrl { get; set; }

        [DataMember(Name = "do_likes")]
        internal int Likes { get; set; }

        [DataMember(Name = "do_pinned")]
        internal bool Pinned { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataPost, Post>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Desription))
                  .ForMember(dst => dst.PostDate, x => x.MapFrom(src => src.PostDate))
                  .ForMember(dst => dst.ExpiryDate, x => x.MapFrom(src => src.ExpiryDate))
                  .ForMember(dst => dst.PostStatus, x => x.MapFrom(src => src.PostStatus))
                  .ForMember(dst => dst.PostType, x => x.MapFrom(src => src.PostType))
                  .ForMember(dst => dst.VideoUrl, x => x.MapFrom(src => src.VideoUrl))
                  .ForMember(dst => dst.Likes, x => x.MapFrom(src => src.Likes))
                 // .ForMember(dst => dst.Pinned, x => x.MapFrom(src => src.Pinned))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
