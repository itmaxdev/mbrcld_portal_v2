import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { SurveyRoutingModule } from './survey-routing.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { SurveyListComponent } from './survey-list/survey-list.component'

@NgModule({
  declarations: [SurveyListComponent],
  imports: [SharedModule, HomeSharedModule, SurveyRoutingModule],
})
export class SurveyModule {}
