import { Component, OnInit, ViewChild, Renderer2 } from '@angular/core'
import {
  FullCalendarComponent,
  DateSelectArg,
  EventClickArg,
  EventApi,
} from '@fullcalendar/angular'
import { CalendarClient, GetUserCalendarViewModel } from 'src/app/shared/api.generated.clients'
import { EventInput, CalendarOptions } from '@fullcalendar/core'
import dayGridPlugin from '@fullcalendar/daygrid'
import * as moment from 'moment'
import { createEventId } from './event-utils'

@Component({
  selector: 'app-calendar-list',
  templateUrl: './calendar-list.component.html',
  styleUrls: ['./calendar-list.component.scss'],
})
export class CalendarListComponent implements OnInit {
  @ViewChild(FullCalendarComponent) calendarComponent!: FullCalendarComponent
  monthNames = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
  ]

  slimS: any
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
    plugins: [dayGridPlugin],
    headerToolbar: {
      start: 'todaysDate prev,next',
      center: '',
      end: 'monthJump',
    },
    customButtons: {
      monthJump: {
        text: '',
        click: () => {},
      },
      todaysDate: {
        text: this.getTodaysDateFormatted(),
      },
    },
    datesSet: () => {
      setTimeout(() => {
        this.insertMonthSelector()
      })
    },
    // eventTimeFormat: {
    // 	hour: 'numeric',
    // 	meridiem: 'short',
    // },
    initialView: 'dayGridMonth',
    timeZone: 'local',
    // initialEvents: this.events,
    //  weekends: true,
    //  editable: true,
    //  selectable: true,
    //  selectMirror: true,
    //  dayMaxEvents: true,
    //  longPressDelay: 200,
    //  eventClick: this.handleEventClick.bind(this),
    //  eventsSet: this.handleEvents.bind(this),
  }

  currentEvents: EventApi[] = []

  constructor(private calendar: CalendarClient, private renderer: Renderer2) {}

  insertMonthSelector() {
    const calendarApi = this.calendarComponent?.getApi()
    if (!calendarApi) return

    const calendarEl = document.querySelector('.fc')
    if (!calendarEl) return

    const button = calendarEl.querySelector('.fc-monthJump-button')
    if (!button) return

    const toolbarCell = button.parentElement
    if (!toolbarCell) return

    let monthSelector = toolbarCell.querySelector('#month-selector') as HTMLSelectElement

    if (!monthSelector) {
      monthSelector = document.createElement('select')
      monthSelector.id = 'month-selector'
      monthSelector.style.padding = '5px 10px'
      monthSelector.style.outline = '0'
      this.monthNames.forEach((name, index) => {
        const option = document.createElement('option')
        option.value = index.toString()
        option.textContent = name
        monthSelector.appendChild(option)
      })

      monthSelector.addEventListener('change', (e: any) => {
        const selectedMonth = parseInt(e.target.value, 10)
        const currentDate = calendarApi.getDate()
        const targetDate = new Date(currentDate.getFullYear(), selectedMonth, 1)
        calendarApi.gotoDate(targetDate)
      })
      toolbarCell.innerHTML = ''
      toolbarCell.appendChild(monthSelector)
    }
    const currentMonth = calendarApi.getDate().getMonth().toString()
    if (monthSelector.value !== currentMonth) {
      monthSelector.value = currentMonth
    }
  }

  getTodaysDateFormatted() {
    const today = new Date()
    const options = {
      day: 'numeric',
      month: 'long',
      year: 'numeric',
    } as any
    return today.toLocaleDateString('en-GB', options)
  }

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
        this.startDate = moment
          .utc(new Date(item.startDate.toString()))
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
          const startDate = moment
            .utc(new Date(item.startDate.toString()))
            .lang('en')
            .format('YYYY-MM-DD HH:mm:ss')
          const endDate = moment
            .utc(new Date(item.endDate.toString()))
            .lang('en')
            .format('YYYY-MM-DD HH:mm:ss')
          this.events.push({
            publicId: item[eventName],
            groupId: eventName,
            title: item.name,
            start: startDate,
            end: endDate,
            allDay: true,
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
