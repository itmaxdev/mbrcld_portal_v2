import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { ViewApplicantPageComponent } from '../programs/view-applicant-page/view-applicant-page.component'
import { MeetingListComponent } from './meeting-list/meeting-list.component'

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: MeetingListComponent,
  },
  {
    path: 'view-applicant/:applicantId',
    component: ViewApplicantPageComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MeetingRoutingModule {}
