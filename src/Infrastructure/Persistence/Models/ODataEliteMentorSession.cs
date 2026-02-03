using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_elitementorsession")]
    internal sealed class ODataEliteMentorSession
    {
        [DataMember(Name = "do_elitementorsessionid")]
        internal Guid EliteMentorSessionId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_date")]
        internal DateTime? Date { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }

        [DataMember(Name = "do_MentorId")]
        internal ODataMentor Mentor { get; set; }

        [DataMember(Name = "do_sessionstatus")]
        internal string SessionStatus { get; set; }


        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEliteMentorSession, EliteMentorSession>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.EliteMentorSessionId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Date, x => x.MapFrom(src => src.Date))
                    .ForMember(dst => dst.ContactId, x => x.MapFrom(src => src.Contact.ContactId))
                    .ForMember(dst => dst.MentorId, x => x.MapFrom(src => src.Mentor.MentorId))
                    .ForMember(dst => dst.MentorName, x => x.MapFrom(src => src.Mentor.Name))
                    .ForMember(dst => dst.SessionStatus, x => x.MapFrom(src => src.SessionStatus))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
