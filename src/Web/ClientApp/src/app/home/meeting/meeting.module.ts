import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { MeetingRoutingModule } from './meeting-routing.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { MeetingListComponent } from './meeting-list/meeting-list.component'
import { TabViewModule } from 'primeng/tabview'

@NgModule({
  declarations: [MeetingListComponent],
  imports: [SharedModule, HomeSharedModule, MeetingRoutingModule, TabViewModule],
})
export class MeetingModule {}
