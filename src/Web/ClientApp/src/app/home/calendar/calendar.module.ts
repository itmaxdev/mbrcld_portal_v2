import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { CalendarListComponent } from './calendar-list/calendar-list.component'
import { CalendarRoutingModule } from './calendar-routing.module'

@NgModule({
  declarations: [CalendarListComponent],
  imports: [CommonModule, CalendarRoutingModule, HomeSharedModule, SharedModule],
})
export class CalendarModule {}
