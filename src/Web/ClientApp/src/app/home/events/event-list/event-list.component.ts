import { Component, Inject, LOCALE_ID, OnInit } from '@angular/core'
import { EventClient, ListEventsViewModel } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-events',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.scss'],
})
export class EventListComponent implements OnInit {
  constructor(private event: EventClient, @Inject(LOCALE_ID) public locale: string) {}

  events: ListEventsViewModel[]
  eventsRegistration: Array<any>
  eventListReady = false
  eventRegistrationReady = false
  test = true
  currentView = 'events'

  async ngOnInit() {
    this.toggleView('events')
  }

  toggleView(view: string) {
    this.currentView = view
    if (view === 'events' && !this.eventListReady) {
      this.fetchEventList()
    } else if (view === 'registration' && !this.eventRegistrationReady) {
      this.fetchEventRegistrationList()
    }
  }

  async fetchEventList() {
    this.event.eventsGet().subscribe((data) => {
      this.events = data
      this.eventListReady = true
      this.test = this.events.length == 0 ? false : true
    })
  }

  async fetchEventRegistrationList() {
    if (!this.eventListReady) {
      this.event.eventsGet().subscribe((data) => {
        this.events = data
        this.eventListReady = true
        this.test = this.events.length == 0 ? false : true
        this.getRegistrationList()
      })
    } else {
      this.getRegistrationList()
    }
  }

  getRegistrationList() {
    this.event.eventsRegistrationGet().subscribe((data) => {
      this.eventsRegistration = data
      this.eventRegistrationReady = true
      for (let x = 0; x < this.eventsRegistration.length; x++) {
        this.eventsRegistration[x].statusCode =
          this.eventsRegistration[x].statusCode == 'Accepted'
            ? 8
            : this.eventsRegistration[x].statusCode == 'Rejected'
            ? 9
            : 4
        this.eventsRegistration[x].endDate = null
        if (this.events) {
          for (let y = 0; y < this.events.length; y++) {
            if (this.events[y].id == this.eventsRegistration[x].eventId) {
              this.eventsRegistration[x].endDate = this.events[y].fromDate
            }
          }
        }
      }
    })
  }

  handleChangeTab(event) {
    switch (event.index) {
      case 0:
        this.toggleView('events')
        break
      case 1:
        this.toggleView('registration')
        break
    }
  }

  getStatusLabel(statusCode: number) {
    switch (statusCode) {
      case 8:
        return this.locale === 'ar' ? 'مقبول' : 'Accepted'
      case 9:
        return this.locale === 'ar' ? 'مرفوض' : 'Rejected'
      case 4:
        return this.locale === 'ar' ? 'قيد المراجعة' : 'Under Review'
      default:
        return this.locale === 'ar' ? 'ملغي' : 'Cancelled'
    }
  }

  getStatusClass(statusCode: number) {
    switch (statusCode) {
      case 8:
        return 'colorSuccess'
      case 9:
        return 'colorDanger'
      case 4:
        return 'colorWarning'
      default:
        return 'colorInfo'
    }
  }
}
