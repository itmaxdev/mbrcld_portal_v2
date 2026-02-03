
namespace Mbrcld.Infrastructure.Persistence.Models
{
    using AutoMapper;
    using Mbrcld.Domain.Entities;
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "do_sector")]
    public class ODataSector
    {
        [DataMember(Name = "do_sectorid")]
        internal Guid SectorId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_name_ar")]
        internal string Name_Ar { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataSector, Sector>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.SectorId))
                    .ForMember(dst => dst.Label, x => x.MapFrom(src => src.Name))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
