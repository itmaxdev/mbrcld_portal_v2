import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { TabViewModule } from 'primeng/tabview'
import { DmCalendarRoutingModule } from './dm-calendar-route.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { TableModule } from 'primeng/table'
import { DmCalendarListComponent } from './dm-calendar-list/dm-calendar-list.component'

@NgModule({
  declarations: [DmCalendarListComponent],
  imports: [DmCalendarRoutingModule, TabViewModule, TableModule, SharedModule, HomeSharedModule],
})
export class DmCalendarModule {}
