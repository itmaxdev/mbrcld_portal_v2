import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { ProfileInstructorRoutingModule } from './profile-instructor-routing.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { KeyFilterModule } from 'primeng/keyfilter'
import { ProfileModule } from './profile.module'

@NgModule({
  imports: [
    ProfileInstructorRoutingModule,
    ProfileModule,
    SharedModule,
    HomeSharedModule,
    KeyFilterModule,
  ],
  declarations: [],
})
export class ProfileInstructorModule {}
