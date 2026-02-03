using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_calendar")]
    internal sealed class ODataCalendar
    {
        [DataMember(Name = "do_calendarid")]
        internal Guid CalendarId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_date")]
        internal DateTime Date { get; set; }

        [DataMember(Name = "do_duration")]
        internal int Duration { get; set; }

        [DataMember(Name = "do_type")]
        internal int Type { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }

        [DataMember(Name = "do_EventId")]
        internal ODataEvent Event { get; set; }

        [DataMember(Name = "do_MeetingId")]
        internal ODataContent Meeting { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataCalendar, Calendar>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.CalendarId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.Date))
                    .ForMember(dst => dst.Type, x => x.MapFrom(src => src.Type))
                    .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                    .ForMember(dst => dst.ContactID, x => x.MapFrom(src => src.Contact.ContactId))
                    .ForMember(dst => dst.EventID, x => x.MapFrom(src => src.Event.EventId))
                    .ForMember(dst => dst.MeetingId, x => x.MapFrom(src => src.Meeting.Id))
                    .ForMember(dst => dst.MeetingUrl, x => x.MapFrom(src => src.Meeting.Url))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
