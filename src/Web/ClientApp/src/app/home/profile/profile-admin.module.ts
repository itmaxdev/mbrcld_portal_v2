import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { ProfileAdminRoutingModule } from './profile-admin-routing.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { KeyFilterModule } from 'primeng/keyfilter'
import { ProfileModule } from './profile.module'
import { TeamComponent } from './team/team.component'

@NgModule({
  imports: [
    ProfileAdminRoutingModule,
    ProfileModule,
    SharedModule,
    HomeSharedModule,
    KeyFilterModule,
  ],
  declarations: [TeamComponent],
})
export class ProfileAdminModule {}
