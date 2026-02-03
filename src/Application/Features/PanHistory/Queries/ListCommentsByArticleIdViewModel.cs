using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListCommentsByArticleIdViewModel
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public string ContactName{ get; set; }
        public Guid? ArticleId { get; set; }
        public int Action { get; set; }
        public int Likes { get; set; }
        public DateTime? ActionDate { get; set; }
        public string Comment { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<PanHistory, ListCommentsByArticleIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.ContactId, x => x.MapFrom(src => src.UserId))
                    .ForMember(dst => dst.ContactName, x => x.MapFrom(src => src.Username))
                    .ForMember(dst => dst.ArticleId, x => x.MapFrom(src => src.ArticleId))
                    .ForMember(dst => dst.Action, x => x.MapFrom(src => src.Action))
                    .ForMember(dst => dst.ActionDate, x => x.MapFrom(src => src.ActionDate))
                    .ForMember(dst => dst.Comment, x => x.MapFrom(src => src.Comment));
            }
        }
        #endregion
    }
}
