using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_eliteclubmember")]
    internal sealed class ODataEliteClubMember
    {
        [DataMember(Name = "do_eliteclubmemberid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }

        [DataMember(Name = "do_EliteClubId")]
        internal ODataEliteClub EliteClub { get; set; }


        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEliteClubMember, EliteClubMember>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.ContactFullName, x => x.MapFrom(src => src.Contact.FirstName + " " + src.Contact.MiddleName + " " + src.Contact.LastName))
                  .ForMember(dst => dst.ContactId, x => x.MapFrom(src => src.Contact.ContactId))
                  .ForMember(dst => dst.PictureUrl, x => x.MapFrom(src => src.Contact.EntityImageKey))
                  .ForMember(dst => dst.EliteClubId, x => x.MapFrom(src => src.EliteClub.Id))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
