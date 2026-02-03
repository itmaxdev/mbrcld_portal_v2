import { GeneralInformationComponent } from './general-information/general-information.component'
import { ProfileComponent } from './profile.component'
import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { TeamComponent } from './team/team.component'

const routes: Routes = [
  {
    path: '',
    component: ProfileComponent,
    canActivate: [],
    children: [
      {
        path: 'general-information',
        component: GeneralInformationComponent,
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'general-information',
      },
      {
        path: 'team',
        component: TeamComponent,
      },
      {
        path: '**',
        redirectTo: 'general-information',
      },
    ],
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProfileInstructorRoutingModule {}
