using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_industry")]
    public class ODataIndustry
    {
        [DataMember(Name = "do_industryid")]
        internal Guid IndustryId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_name_ar")]
        internal string Name_Ar { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataIndustry, Industry>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.IndustryId))
                    .ForMember(dst => dst.Label, x => x.MapFrom(src => src.Name))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
