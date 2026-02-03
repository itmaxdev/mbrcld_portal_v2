using AutoMapper;
using ChatData.Models;
using Mbrcld.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Mbrcld.Application.Features.Chat.Queries
{
    public sealed class GetUserRoomsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ChatAvatar { get; set; }
        public int UnreadMessagesCount { get; set; }
        public Message LastMessage { get; set; }
        public List<ParticipantInfo> Participants { get; set; }

        public sealed class ParticipantInfo
        {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
            public string FullNameAr { get; set; }
            public string UserAvater { get; set; }
        }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Room, GetUserRoomsViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.Image, x => x.MapFrom(src => src.Image))
                    .ForMember(dst => dst.Participants, x => x.MapFrom(src => src.Participants));

                CreateMap<Participants, ParticipantInfo>()
                    .ForMember(dst => dst.UserId, x => x.MapFrom(src => src.UserId))
                    .ForMember(dst => dst.FullNameAr, x => x.MapFrom(src => src.FullNameAr))
                    .ForMember(dst => dst.FullName, x => x.MapFrom(src => src.FullName))
                    .ForMember(dst => dst.UserAvater, x => x.MapFrom(src => src.UserAvater));
            }
        }
        #endregion
    }
}
