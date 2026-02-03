using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class SearchUserArticlesViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TheArticle { get; set; }
        public int ArticleStatus { get; set; }
        public DateTime Date { get; set; }
        public Guid WrittenBy { get; set; }
        public string WrittenByName { get; set; }
        public string PictureUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool Liked { get; set; }
        public int? Likes { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Article, SearchUserArticlesViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Date, x => x.MapFrom(src => src.Date))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.WrittenBy, x => x.MapFrom(src => src.WrittenBy))
                    .ForMember(dst => dst.WrittenByName, x => x.MapFrom(src => src.WrittenByName))
                    .ForMember(dst => dst.ArticleStatus, x => x.MapFrom(src => src.ArticleStatus))
                    .ForMember(dst => dst.TheArticle, x => x.MapFrom(src => src.TheArticle));
            }
        }
        #endregion
    }
}
