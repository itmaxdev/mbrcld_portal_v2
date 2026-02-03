import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { DmCalendarListComponent } from './dm-calendar-list/dm-calendar-list.component'
const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: DmCalendarListComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class DmCalendarRoutingModule {}
