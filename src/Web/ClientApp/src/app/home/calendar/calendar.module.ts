import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { CalendarListComponent } from './calendar-list/calendar-list.component'
import { CalendarRoutingModule } from './calendar-routing.module'
import { DialogModule } from 'primeng/dialog'

@NgModule({
  declarations: [CalendarListComponent],
  imports: [DialogModule, CommonModule, CalendarRoutingModule, HomeSharedModule, SharedModule],
})
export class CalendarModule {}
