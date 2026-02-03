using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_elitemembership")]
    internal sealed class ODataEliteMembership
    {
        [DataMember(Name = "do_elitemembershipid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_fromdate")]
        internal DateTime? FromDate { get; set; }

        [DataMember(Name = "do_todate")]
        internal DateTime? ToDate { get; set; }

        [DataMember(Name = "do_description")]
        internal string Decription { get; set; }

        [DataMember(Name = "do_type")]
        internal int Type { get; set; }

        [DataMember(Name = "do_EliteClubId")]
        internal ODataEliteClub EliteClub { get; set; }


        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEliteMembership, EliteMembership>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.FromDate, x => x.MapFrom(src => src.FromDate))
                  .ForMember(dst => dst.ToDate, x => x.MapFrom(src => src.ToDate))
                  .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Decription))
                  .ForMember(dst => dst.Type, x => x.MapFrom(src => src.Type))
                  .ForMember(dst => dst.EliteClubId, x => x.MapFrom(src => src.EliteClub.Id))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
