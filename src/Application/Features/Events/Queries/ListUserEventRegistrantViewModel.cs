using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static Mbrcld.Domain.Entities.EventRegistrant;

namespace Mbrcld.Application.Features.Events.Queries
{
    public sealed class ListUserEventRegistrantViewModel
    {
        public Guid Registrant { get; set; }
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string StatusCode { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EventRegistrant, ListUserEventRegistrantViewModel>()
                    .ForMember(dst => dst.Registrant, x => x.MapFrom(src => src.UserId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.StatusCode, x => x.MapFrom(src => src.StatusCode));
            }
        }
        #endregion
    }
}
