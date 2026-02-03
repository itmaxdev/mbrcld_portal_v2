namespace Mbrcld.Infrastructure.Persistence.Models
{
    using AutoMapper;
    using Mbrcld.Domain.Entities;
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "do_skillandinterest")]
    public class ODataSkillAndInterest
    {
        [DataMember(Name = "do_skillandinterestid")]
        internal Guid Skills_InterestId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact ContactId { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataSkillAndInterest, Interest>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Skills_InterestId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.ContactId, x => x.MapFrom(src => src.ContactId.ContactId))
                    .ReverseMap();
            }
        }
        #endregion
    }
}