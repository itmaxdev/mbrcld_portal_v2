using AutoMapper;
using ChatData.Models;
using System;

namespace Mbrcld.Application.Hubs
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public MessageTypes MessageType { get; set; }
        public DateTime DateTime { get; set; }
        public string ConnectionId { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ChatMessage, Message>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Text, x => x.MapFrom(src => src.Text))
                    .ForMember(dst => dst.RoomId, x => x.MapFrom(src => src.RoomId))
                    .ForMember(dst => dst.UserId, x => x.MapFrom(src => src.UserId))
                    .ForMember(dst => dst.Time, x => x.MapFrom(src => src.DateTime))
                    .ForMember(dst => dst.MessageType, x => x.MapFrom(src => src.MessageType));
            }
        }
        #endregion
    }
}
