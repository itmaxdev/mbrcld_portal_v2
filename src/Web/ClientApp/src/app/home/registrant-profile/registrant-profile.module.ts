import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { RouterModule, Routes } from '@angular/router'
import { RegistrantProfileComponent } from './registrant-profile.component'
import { RegistrantGeneralInformationComponent } from './components/general-information/general-information.component'
import { RegistrantIdentityComponent } from './components/identity/identity.component'
import { RegistrantExperienceComponent } from './components/experience/experience.component'
import { RegistrantEducationComponent } from './components/education/education.component'
import { RegistrantTrainingCoursesComponent } from './components/training-courses/training-courses.component'
import { RegistrantAchievementsComponent } from './components/achievements/achievements.component'
import { RegistrantPreferencesComponent } from './components/preferences/preferences.component'
import { RegistrantSkillsComponent } from './components/skills/skills.component'
import { RegistrantModuleTeamComponent } from './components/module-team/module-team.component'

const routes: Routes = [
  {
    path: '',
    component: RegistrantProfileComponent,
    children: [
      {
        path: '',
        redirectTo: 'general-information',
        pathMatch: 'full',
      },
      {
        path: 'general-information',
        component: RegistrantGeneralInformationComponent,
      },
      {
        path: 'identity',
        component: RegistrantIdentityComponent,
      },
      {
        path: 'experience',
        component: RegistrantExperienceComponent,
      },
      {
        path: 'education',
        component: RegistrantEducationComponent,
      },
      {
        path: 'training-courses',
        component: RegistrantTrainingCoursesComponent,
      },
      {
        path: 'achievements',
        component: RegistrantAchievementsComponent,
      },
      {
        path: 'preferences',
        component: RegistrantPreferencesComponent,
      },
      {
        path: 'skills',
        component: RegistrantSkillsComponent,
      },
      {
        path: 'module-team',
        component: RegistrantModuleTeamComponent,
      },
    ],
  },
]

@NgModule({
  declarations: [
    RegistrantProfileComponent,
    RegistrantGeneralInformationComponent,
    RegistrantIdentityComponent,
    RegistrantExperienceComponent,
    RegistrantEducationComponent,
    RegistrantTrainingCoursesComponent,
    RegistrantAchievementsComponent,
    RegistrantPreferencesComponent,
    RegistrantSkillsComponent,
    RegistrantModuleTeamComponent,
  ],
  imports: [CommonModule, RouterModule.forChild(routes)],
})
export class RegistrantProfileModule {}
