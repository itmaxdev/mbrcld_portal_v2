namespace Mbrcld.Infrastructure.Persistence.Models
{
    using AutoMapper;
    using Mbrcld.Domain.Entities;
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "do_languagelevel")]
    public class ODataLanguageSkill
    {
        [DataMember(Name = "do_languagelevelid")]
        internal Guid LanguageSkillId { get; set; }

        [DataMember(Name = "do_level")]
        internal int Level { get; set; }

        [DataMember(Name = "do_LanguageId")]
        internal ODataLanguage LanguageId { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact ContactId { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataLanguageSkill, LanguageSkill>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.LanguageSkillId))
                    .ForMember(dst => dst.Level, x => x.MapFrom(src => src.Level))
                    .ForMember(dst => dst.LanguageId, x => x.MapFrom(src => src.LanguageId.LanguageId))
                    .ForMember(dst => dst.ContactId, x => x.MapFrom(src => src.ContactId.ContactId))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
