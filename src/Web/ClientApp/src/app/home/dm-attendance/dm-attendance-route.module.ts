import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { DmAttendanceListComponent } from './dm-attendance-list/dm-attendance-list.component'
const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: DmAttendanceListComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class DmAttendanceRoutingModule {}
