import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { TabViewModule } from 'primeng/tabview'
import { DmDashboardRoutingModule } from './dm-dashboard-route.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { TableModule } from 'primeng/table'
import { DmDashboardListComponent } from './dm-dashboard-list/dm-dashboard-list.component'

@NgModule({
  declarations: [DmDashboardListComponent],
  imports: [SharedModule, TabViewModule, HomeSharedModule, TableModule, DmDashboardRoutingModule],
})
export class DmDashboardModule {}
