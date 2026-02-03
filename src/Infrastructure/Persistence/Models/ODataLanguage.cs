using System;
using System.Collections.Generic;
using System.Text;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    using AutoMapper;
    using Mbrcld.Domain.Entities;
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "do_language")]
    public class ODataLanguage
    {
        [DataMember(Name = "do_languageid")]
        internal Guid LanguageId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_name_ar")]
        internal string Name_Ar { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataLanguage, Language>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.LanguageId))
                    .ForMember(dst => dst.Label, x => x.MapFrom(src => src.Name))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
