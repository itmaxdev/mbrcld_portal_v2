using AutoMapper;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class CalendarRepository : ICalendarRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public CalendarRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<IList<Calendar>> GetUserCalendarAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            //List<ODataCalendar> calendars = new List<ODataCalendar>();
            var odataUserCalendar = await this.webApiClient.For<ODataCalendar>()
                .Filter(x => x.Contact.ContactId == userId)
                .OrderByDescending(x => x.Date)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            //foreach (var calendar in odataUserCalendar)
            //{
            //    if (calendar.Type == 1) //Event
            //    {
            //        calendar.Name = calendar.Event.Name;
            //    }
            //    if (calendar.Type == 2) //Meeting
            //    {
            //        calendar.Name = calendar.Meeting.Name;
            //    }
            //    calendars.Add(calendar);
            //}

            var userCalendars = this.mapper.Map<IList<Calendar>>(odataUserCalendar);
            foreach (var usercalendar in userCalendars)
            {
                usercalendar.EndDate = usercalendar.StartDate.AddMinutes(usercalendar.Duration);
            }
            return userCalendars;
        }

        public async Task<IList<Calendar>> ListApplicantMeetingAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            //List<ODataCalendar> calendars = new List<ODataCalendar>();
            var odataUserCalendar = await this.webApiClient.For<ODataCalendar>()
                .Filter(x => x.Contact.ContactId == userId)
                .Filter(x => x.Type == 2)//Meeting
                .OrderByDescending(x => x.Date)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            //foreach (var calendar in odataUserCalendar)
            //{
            //    if (calendar.Type == 2) //Meeting
            //    {
            //        calendar.Name = calendar.Meeting.Name;
            //    }
            //    calendars.Add(calendar);
            //}

            var userCalendars = this.mapper.Map<IList<Calendar>>(odataUserCalendar);
            foreach (var usercalendar in userCalendars)
            {
                usercalendar.EndDate = usercalendar.StartDate.AddMinutes(usercalendar.Duration);
            }
            return userCalendars;
        }
    }
}
