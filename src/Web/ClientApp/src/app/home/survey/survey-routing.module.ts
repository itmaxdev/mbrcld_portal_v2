import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { SurveyListComponent } from './survey-list/survey-list.component'

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: SurveyListComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SurveyRoutingModule {}
