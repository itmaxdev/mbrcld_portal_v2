import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { TabViewModule } from 'primeng/tabview'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { DashboardRoutingModule } from './dashboard-routing.module'
import { DashboardListComponent } from './dashboard-list/dashboard-list.component'

@NgModule({
  declarations: [DashboardListComponent],
  imports: [CommonModule, HomeSharedModule, DashboardRoutingModule, SharedModule, TabViewModule],
})
export class DashboardModule {}
