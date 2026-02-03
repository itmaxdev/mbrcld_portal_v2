using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class SearchPostsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string PictureUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool Liked { get; set; }
        public int? Likes { get; set; }
        public string WrittenByName { get; set; }
        public Guid WrittenBy { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public bool AdminArticle { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Post, SearchPostsViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.ExpiryDate, x => x.MapFrom(src => src.ExpiryDate))
                    .ForMember(dst => dst.PostDate, x => x.MapFrom(src => src.PostDate))
                    .ForMember(dst => dst.WrittenBy, x => x.MapFrom(src => src.WrittenBy))
                    .ForMember(dst => dst.AdminArticle, x => x.MapFrom(src => src.AdminArticle))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description));
            }
        }
        #endregion
    }
}
