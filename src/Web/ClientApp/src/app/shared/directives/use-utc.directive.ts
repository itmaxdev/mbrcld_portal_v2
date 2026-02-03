import { Directive, Host, HostListener, Self } from '@angular/core'
import * as moment from 'moment'
import { Calendar } from 'primeng/calendar'

@Directive({
  selector: '[appUseUtc]',
})
export class UseUtcDirective {
  constructor(@Host() @Self() private calendar: Calendar) {}

  @HostListener('onSelect')
  onSelect() {
    this.convertValueToUtc()
  }

  @HostListener('onInput')
  onInput() {
    this.convertValueToUtc()
  }

  private convertValueToUtc() {
    const utcValue = moment(this.calendar.value).utc(true).toDate()
    this.calendar.updateModel(utcValue)
  }
}
