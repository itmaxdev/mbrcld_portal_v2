import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { TabViewModule } from 'primeng/tabview'
import { DmApplicantsRoutingModule } from './dm-applicants-route.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { DmApplicantsViewComponent } from './dm-applicants-view/dm-applicants-view.component'

@NgModule({
  declarations: [DmApplicantsViewComponent],
  imports: [SharedModule, TabViewModule, HomeSharedModule, DmApplicantsRoutingModule],
})
export class DmApplicantsModule {}
