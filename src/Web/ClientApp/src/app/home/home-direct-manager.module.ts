import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeDirectManagerRoutingModule } from './home-direct-manager-routing.component'
import { HomeModule } from './home.module'
import { HomeSharedModule } from './shared/home-shared.module'
import { DmApplicantsListComponent } from './dm-applicants/dm-applicants-list/dm-applicants-list.component'

@NgModule({
  imports: [HomeDirectManagerRoutingModule, HomeModule, SharedModule, HomeSharedModule],
  declarations: [DmApplicantsListComponent],
})
export class HomeDirectManagerModule {}
