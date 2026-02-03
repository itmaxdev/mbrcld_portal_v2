using AutoMapper;
using ChatData.Models;
using System;

namespace Mbrcld.Application.Features.Chat.Queries
{
    public class GetRoomMessageViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public MessageTypes MessageType { get; set; }
        public DateTime Time { get; set; }
        public bool IsReaded { get; set; }
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public File File { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Message, GetRoomMessageViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Text, x => x.MapFrom(src => src.Text))
                    .ForMember(dst => dst.Time, x => x.MapFrom(src => src.Time))
                    .ForMember(dst => dst.RoomId, x => x.MapFrom(src => src.RoomId))
                    .ForMember(dst => dst.UserId, x => x.MapFrom(src => src.UserId))
                    .ForMember(dst => dst.MessageType, x => x.MapFrom(src => src.MessageType))
                    .ForMember(dst => dst.File, x => x.MapFrom(src => src.File));
            }
        }
        #endregion
    }
}
