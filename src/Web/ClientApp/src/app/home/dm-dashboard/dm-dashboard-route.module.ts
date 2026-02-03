import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { DmDashboardListComponent } from './dm-dashboard-list/dm-dashboard-list.component'
const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: DmDashboardListComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class DmDashboardRoutingModule {}
