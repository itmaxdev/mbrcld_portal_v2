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

  async ngOnInit() {
    this.fetchEventList()
  }

  async fetchEventList() {
    this.event.eventsGet().subscribe((data) => {
      this.events = data
      this.eventListReady = true
      this.test = this.events.length == 0 ? false : true
    })
  }

  async fetchEventRegistrationList() {
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
        this.fetchEventList()
        break
      case 1:
        this.fetchEventRegistrationList()
        break
    }
  }
}
