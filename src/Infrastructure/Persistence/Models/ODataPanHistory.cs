using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_panhistory")]
    internal sealed class ODataPanHistory
    {
        [DataMember(Name = "do_panhistoryid")]
        internal Guid PanHistoryId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_action")]
        internal int Action { get; set; }

        [DataMember(Name = "do_dateofaction")]
        internal DateTime? ActionDate { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact ContactId { get; set; }

        [DataMember(Name = "do_ArticleId")]
        internal ODataArticle ArticleId { get; set; }

        [DataMember(Name = "do_PostId")]
        internal ODataPost PostId { get; set; }

        [DataMember(Name = "do_NewsfeedId")]
        internal ODataNewsFeed NewsFeedId { get; set; }

        [DataMember(Name = "do_comment")]
        internal string Comment { get; set; }

        [DataMember(Name = "createdon")]
        public DateTime? CreatedOn { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataPanHistory, PanHistory>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.PanHistoryId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.UserId, x => x.MapFrom(src => src.ContactId.ContactId))
                    .ForMember(dst => dst.Username, x => x.MapFrom(src => src.ContactId.FirstName + " " + src.ContactId.MiddleName + " " + src.ContactId.LastName))
                    .ForMember(dst => dst.ArticleId, x => x.MapFrom(src => src.ArticleId.Id))
                    .ForMember(dst => dst.PostId, x => x.MapFrom(src => src.PostId.Id))
                    .ForMember(dst => dst.NewsFeedId, x => x.MapFrom(src => src.NewsFeedId.Id))
                    .ForMember(dst => dst.Action, x => x.MapFrom(src => src.Action))
                    .ForMember(dst => dst.Comment, x => x.MapFrom(src => src.Comment))
                    .ForMember(dst => dst.ActionDate, x => x.MapFrom(src => src.ActionDate))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
