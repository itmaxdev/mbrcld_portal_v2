import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { EventsRoutingModule } from './events-routing.module'
import { EventListComponent } from './event-list/event-list.component'
import { HomeSharedModule } from '../shared/home-shared.module'
import { EventCardComponent } from './event-card/event-card.component'
import { EventDetailsComponent } from './event-details/event-details.component'
import { TabViewModule } from 'primeng/tabview'
@NgModule({
  declarations: [EventListComponent, EventCardComponent, EventDetailsComponent],
  imports: [TabViewModule, SharedModule, HomeSharedModule, EventsRoutingModule],
})
export class EventsModule {}
