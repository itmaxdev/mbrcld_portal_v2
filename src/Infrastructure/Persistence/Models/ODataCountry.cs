using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_countries")]
    internal sealed class ODataCountry
    {
        [DataMember(Name = "do_countryid")]
        internal Guid CountryId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_isocode")]
        internal string IsoCode { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataCountry, Country>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.CountryId))
                    .ForMember(dst => dst.IsoCode, x => x.MapFrom(src => src.IsoCode))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
