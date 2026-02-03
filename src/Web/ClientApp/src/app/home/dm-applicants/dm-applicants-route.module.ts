import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { DmApplicantsListComponent } from './dm-applicants-list/dm-applicants-list.component'
import { ViewApplicantPageComponent } from '../programs/view-applicant-page/view-applicant-page.component'
const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: DmApplicantsListComponent,
  },
  // {
  //   path: 'view-applicant/:applicantId',
  //   component: ViewApplicantPageComponent,
  // },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class DmApplicantsRoutingModule {}
