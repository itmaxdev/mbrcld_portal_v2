import { Component, OnInit } from '@angular/core'
import { CalendarOptions, EventApi, EventClickArg, EventInput } from '@fullcalendar/common'
import {
  CalendarClient,
  GetUserCalendarViewModel,
  ProfileClient,
} from 'src/app/shared/api.generated.clients'
import * as moment from 'moment'

export interface Product {
  name?: string
  profilePictureUrl?: string
}

@Component({
  selector: 'app-dm-calendar-list',
  templateUrl: './dm-calendar-list.component.html',
  styleUrls: ['./dm-calendar-list.component.scss'],
})
export class DmCalendarListComponent implements OnInit {
  products: any[]
  startDate: string
  title: string
  duration: number
  selectedProduct: any
  events: EventInput[] = []
  calendarOptions: CalendarOptions = {
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek',
    },
    initialView: 'dayGridMonth',
    initialEvents: this.events,
    weekends: true,
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    eventClick: this.handleEventClick.bind(this),
    eventsSet: this.handleEvents.bind(this),
  }

  readyEventInfo = false
  clickedOnEvent = false
  eventsData: GetUserCalendarViewModel[]
  isReadyCalendarData = false
  isReady = false
  currentEvents: EventApi[] = []

  constructor(private profile: ProfileClient, private calendar: CalendarClient) {}

  ngOnInit(): void {
    this.profile.directmanagerApplicants().subscribe((data) => {
      this.products = data
      for (let count = 0; count < this.products.length; count++) {
        this.products[count].isActive = false
      }
      this.isReady = true
    })
  }

  handleEventClick(clickInfo: EventClickArg) {
    this.readyEventInfo = false
    this.eventsData.map((item) => {
      if (item[clickInfo.event.groupId] === clickInfo.event.extendedProps.publicId) {
        this.title = item.name
        this.duration = item.duration
        this.startDate = moment(new Date(item.startDate.toString()))
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

  onApplicantSelect(id) {
    this.clickedOnEvent = false
    this.selectedProduct = true
    this.isReadyCalendarData = false
    if (id) {
      for (let count = 0; count < this.products.length; count++) {
        if (this.products[count].id === id) {
          this.products[count].isActive = true
        } else {
          this.products[count].isActive = false
        }
      }
      this.calendar.calendar(id).subscribe((data) => {
        this.eventsData = data
        data.map((item) => {
          // const date = moment(item.date).lang('en').format('YYYY-MM-DD')
          let eventName = 'eventID'
          if (item.type == 2) {
            eventName = 'meetingId'
          }
          this.events.push({
            publicId: item[eventName],
            groupId: eventName,
            title: item.name,
            start: item.startDate,
            end: item.endDate,
          })
        })

        this.calendarOptions.initialEvents = this.events
        this.isReadyCalendarData = true

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
      })
    } else {
      this.selectedProduct = undefined
    }
  }
}
