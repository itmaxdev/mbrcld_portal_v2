import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import Datepicker from 'vanillajs-datepicker/js/Datepicker'

@Component({
  selector: 'app-side-box',
  templateUrl: './sideBox.component.html',
  styleUrls: ['./sideBox.component.scss'],
})
export class SideBoxComponent {
  @ViewChild('inlineCalenderSmall', { static: false })
  calendarEl!: ElementRef

  private eventDates: string[] = [
    '2025-11-05',
    '2025-11-15',
    '2025-11-25',
    '2025-12-14',
    '2025-12-11',
    '2025-12-24',
  ]

  constructor() {}

  ngOnInit() {}

  ngAfterViewInit(): void {
    if (!this.calendarEl) return
    new Datepicker(this.calendarEl.nativeElement, {
      autohide: false,
      format: 'dd/mm/yyyy',
      beforeShowDay: this.highlightEvents.bind(this),
    })
  }

  private highlightEvents(date: Date) {
    const today = new Date()

    const todayString = this.formatDate(today)
    const dateString = this.formatDate(date)

    const classes: string[] = []
    let isHighlighted = false

    if (dateString === todayString) {
      classes.push('todayHighlight')
      isHighlighted = true
    }

    if (this.eventDates.includes(dateString)) {
      classes.push('eventHighlight')
      isHighlighted = true
    }

    if (isHighlighted) {
      return {
        classes: classes.join(' '),
        content: `<span>${date.getDate()}</span>`,
      }
    }

    return
  }

  private formatDate(date: Date): string {
    const yyyy = date.getFullYear()
    const mm = String(date.getMonth() + 1).padStart(2, '0')
    const dd = String(date.getDate()).padStart(2, '0')
    return `${yyyy}-${mm}-${dd}`
  }
}
