import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { RegistrantProfileComponent } from '../registrant-profile/registrant-profile.component'
import { RegistrantGeneralInformationComponent } from '../registrant-profile/components/general-information/general-information.component'
import { RegistrantModuleTeamComponent } from '../registrant-profile/components/module-team/module-team.component'

const routes: Routes = [
  {
    path: '',
    component: RegistrantProfileComponent,
    canActivate: [],
    children: [
      {
        path: 'general-information',
        component: RegistrantGeneralInformationComponent,
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'general-information',
      },
      {
        path: 'module-team',
        component: RegistrantModuleTeamComponent,
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
