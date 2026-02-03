using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListEliteMentorSessionsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public Guid MentorId { get; set; }
        public string MentorName { get; set; }
        public string MentorProfilePicture { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EliteMentorSession, ListEliteMentorSessionsViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Date, x => x.MapFrom(src => src.Date))
                    .ForMember(dst => dst.MentorId, x => x.MapFrom(src => src.MentorId))
                    .ForMember(dst => dst.MentorName, x => x.MapFrom(src => src.MentorName));
            }
        }
        #endregion
    }
}
