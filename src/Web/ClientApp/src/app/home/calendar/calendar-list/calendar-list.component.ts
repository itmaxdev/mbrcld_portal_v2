import { Component, OnInit, Renderer2 } from '@angular/core'
import { CalendarOptions, DateSelectArg, EventClickArg, EventApi } from '@fullcalendar/angular'
import { CalendarClient, GetUserCalendarViewModel } from 'src/app/shared/api.generated.clients'
import { EventInput } from '@fullcalendar/angular'
import * as moment from 'moment'
import { createEventId } from './event-utils'

@Component({
  selector: 'app-calendar-list',
  templateUrl: './calendar-list.component.html',
  styleUrls: ['./calendar-list.component.scss'],
})
export class CalendarListComponent implements OnInit {
  link: string
  startDate: string
  title: string
  eventInfo: any
  duration: number
  ready = false
  calendarVisible = true
  events: EventInput[] = []
  readyEventInfo = false
  clickedOnEvent = false
  eventsData: GetUserCalendarViewModel[]
  calendarOptions: CalendarOptions = {
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek',
    },
    eventTimeFormat: {
      hour: 'numeric',
      meridiem: 'short',
    },
    initialView: 'dayGridMonth',
    timeZone: 'local',
    initialEvents: this.events,
    weekends: true,
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    longPressDelay: 200,
    eventClick: this.handleEventClick.bind(this),
    eventsSet: this.handleEvents.bind(this),
    // dayCellDidMount: ({ date, el, view }) => {
    //   const eventsWithDate = this.events.filter(event => {
    //     return moment(event.start).toDate() >= date || moment(event.end).toDate() <= date
    //   });

    //   if (eventsWithDate.length > 0) {
    //     el.style.backgroundColor = '#f2f2f2';
    //   }

    // },
    // eventDidMount : (eventInfo)=>{
    //   eventInfo.el.parentElement.parentElement.parentElement.parentElement.style.backgroundColor = '#f2f2f2';
    // },
    
  }

  currentEvents: EventApi[] = []

  constructor(private calendar: CalendarClient, private renderer: Renderer2) {}

  handleCalendarToggle() {
    this.calendarVisible = !this.calendarVisible
  }

  handleWeekendsToggle() {
    const { calendarOptions } = this
    calendarOptions.weekends = !calendarOptions.weekends
  }

  handleDateSelect(selectInfo: DateSelectArg) {
    const title = prompt('Please enter a new title for your event')
    const calendarApi = selectInfo.view.calendar

    calendarApi.unselect() // clear date selection

    if (title) {
      calendarApi.addEvent({
        id: createEventId(),
        title,
        start: selectInfo.startStr,
        end: selectInfo.startStr,
        allDay: selectInfo.allDay,
      })
    }
  }

  handleEventClick(clickInfo: EventClickArg) {
    this.readyEventInfo = false
    this.eventsData.map((item) => {
      if (item[clickInfo.event.groupId] === clickInfo.event.extendedProps.publicId) {
        this.title = item.name
        this.duration = item.duration
        this.startDate = moment.utc(new Date(item.startDate.toString()))
          .lang('en')
          .format('dddd, MMMM D')
      }
    })
    this.readyEventInfo = true
    this.clickedOnEvent = true
  }

  handleEvents(events: EventApi[]) {
    this.currentEvents = events
  }

  async ngOnInit() {
    this.ready = false
    await Promise.all([
      this.calendar.calendar(undefined).subscribe((data) => {
        this.eventsData = data
        data.map((item) => {
          let eventName = 'eventID'
          if (item.type == 2) {
            eventName = 'meetingId'
          }
          var startDate = moment.utc(new Date(item.startDate.toString())).lang('en').format('YYYY-MM-DD HH:mm:ss');
          var endDate = moment.utc(new Date(item.endDate.toString())).lang('en').format('YYYY-MM-DD HH:mm:ss');
          this.events.push({
            publicId: item[eventName],
            groupId: eventName,
            title: item.name,
            start: startDate,
            end: endDate,
          })
        })

        this.calendarOptions.initialEvents = this.events
        this.ready = true

        setTimeout(() => {
          document
            .getElementsByClassName('fc-timeGridDay-button')[0]
            .addEventListener('click', () => {
              document.getElementsByClassName('fc-scroller-liquid-absolute')[0].scroll(0, 350)
            })
          document
            .getElementsByClassName('fc-timeGridWeek-button')[0]
            .addEventListener('click', () => {
              document.getElementsByClassName('fc-scroller-liquid-absolute')[0].scroll(0, 350)
            })
        }, 500)
      }),
    ])
  }
}
