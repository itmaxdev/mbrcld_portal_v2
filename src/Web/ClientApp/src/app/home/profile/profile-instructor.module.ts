import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { ProfileInstructorRoutingModule } from './profile-instructor-routing.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { KeyFilterModule } from 'primeng/keyfilter'
import { ProfileModule } from './profile.module'
import { TeamComponent } from './team/team.component'

@NgModule({
  imports: [
    ProfileInstructorRoutingModule,
    ProfileModule,
    SharedModule,
    HomeSharedModule,
    KeyFilterModule,
  ],
  declarations: [TeamComponent],
})
export class ProfileInstructorModule {}
