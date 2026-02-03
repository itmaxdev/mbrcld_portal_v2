using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "systemuser")]
    internal sealed class ODataSystemUser
    {
        [DataMember(Name = "systemuserid")]
        internal Guid SystemUserId { get; set; }

        [DataMember(Name = "fullname")]
        internal string FullName { get; set; }


        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataSystemUser, SystemUser>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.SystemUserId))
                    .ForMember(dst => dst.FullName, x => x.MapFrom(src => src.FullName))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
