using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_article")]
    internal sealed class ODataArticle
    {
        [DataMember(Name = "do_articleid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_WrittenById")]
        internal ODataContact WrittenBy { get; set; }

        [DataMember(Name = "do_description")]
        internal string Desription { get; set; }
        
        [DataMember(Name = "do_thearticle")]
        internal string TheArticle { get; set; }

        [DataMember(Name = "do_articlestatus")]
        internal int? ArticlesStatus { get; set; }

        [DataMember(Name = "do_date")]
        internal DateTime? Date { get; set; }

        [DataMember(Name = "createdon")]
        public DateTime? CreatedOn { get; set; }

        [DataMember(Name = "OwningUser")]
        internal ODataSystemUser Owner { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataArticle, Article>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.WrittenBy, x => x.MapFrom(src => src.WrittenBy.ContactId))
                  .ForMember(dst => dst.WrittenByName, x => x.MapFrom(src => src.WrittenBy.FirstName + " " + src.WrittenBy.MiddleName + " " + src.WrittenBy.LastName))
                  .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Desription))
                  .ForMember(dst => dst.TheArticle, x => x.MapFrom(src => src.TheArticle))
                  .ForMember(dst => dst.ArticleStatus, x => x.MapFrom(src => src.ArticlesStatus))
                  .ForMember(dst => dst.Date, x => x.MapFrom(src => src.Date))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
