using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_eliteclub")]
    internal sealed class ODataEliteClub
    {
        [DataMember(Name = "do_eliteclubid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_fromdate")]
        internal DateTime? FromDate { get; set; }

        [DataMember(Name = "do_todate")]
        internal DateTime? ToDate { get; set; }

        [DataMember(Name = "do_overview")]
        internal string Overview { get; set; }


        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEliteClub, EliteClub>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.FromDate, x => x.MapFrom(src => src.FromDate))
                  .ForMember(dst => dst.ToDate, x => x.MapFrom(src => src.ToDate))
                  .ForMember(dst => dst.Overview, x => x.MapFrom(src => src.Overview))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
