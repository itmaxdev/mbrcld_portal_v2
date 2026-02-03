using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListArticlesViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string PictureUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
        public Guid WrittenBy { get; set; }
        public string WrittenByName { get; set; }
        public bool Liked { get; set; }
        public bool AdminArticle { get; set; }
        public int? Likes { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Article, ListArticlesViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Date, x => x.MapFrom(src => src.Date))
                    .ForMember(dst => dst.WrittenBy, x => x.MapFrom(src => src.WrittenBy))
                    .ForMember(dst => dst.WrittenByName, x => x.MapFrom(src => src.WrittenByName))
                    .ForMember(dst => dst.AdminArticle, x => x.MapFrom(src => src.AdminArticle))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description));
            }
        }
        #endregion
    }
}
