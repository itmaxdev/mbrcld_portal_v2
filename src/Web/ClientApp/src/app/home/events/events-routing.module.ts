import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { EventListComponent } from './event-list/event-list.component'
import { EventDetailsComponent } from './event-details/event-details.component'

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: EventListComponent,
  },
  {
    path: ':id',
    component: EventDetailsComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EventsRoutingModule {}
