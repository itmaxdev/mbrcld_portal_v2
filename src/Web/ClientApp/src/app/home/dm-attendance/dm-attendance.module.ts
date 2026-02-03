import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { TabViewModule } from 'primeng/tabview'
import { TableModule } from 'primeng/table'
import { DmAttendanceRoutingModule } from './dm-attendance-route.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { DmAttendanceListComponent } from './dm-attendance-list/dm-attendance-list.component'

@NgModule({
  declarations: [DmAttendanceListComponent],
  imports: [DmAttendanceRoutingModule, TabViewModule, SharedModule, TableModule, HomeSharedModule],
})
export class DmAttendanceModule {}
