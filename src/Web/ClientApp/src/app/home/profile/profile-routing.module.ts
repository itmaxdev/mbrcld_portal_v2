import { ContactDetailsComponent } from './contact-details/contact-details.component'
import { EducationComponent } from './education/education.component'
import { GeneralInformationApplicantComponent } from './general-information-applicant/general-information-applicant.component'
import { IdentityAndDocumentsComponent } from './identity-and-documents/identity-and-documents.component'
import { LanguagesComponent } from './languages/languages.component'
import { LearningPreferencesComponent } from './learning-preferences/learning-preferences.component'
import { MembershipsComponent } from './memberships/memberships.component'
import { ProfessionalExperienceComponent } from './professional-experience/professional-experience.component'
import { ProfileComponent } from './profile.component'
import { SkillsAndInterestsComponent } from './skills-and-interests/skills-and-interests.component'
import { ProfileAchievementsComponent } from './profile-achievements/profile-achievements.component'
import { TrainingCoursesComponent } from './training-courses/training-courses.component'
import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'

const routes: Routes = [
  {
    path: '',
    component: ProfileComponent,
    canActivate: [],
    children: [
      {
        path: 'general-information',
        component: GeneralInformationApplicantComponent,
      },
      {
        path: 'contact-details',
        component: ContactDetailsComponent,
      },
      {
        path: 'identity',
        component: IdentityAndDocumentsComponent,
      },
      {
        path: 'professional-experience',
        component: ProfessionalExperienceComponent,
      },
      {
        path: 'education',
        component: EducationComponent,
      },
      {
        path: 'training',
        component: TrainingCoursesComponent,
      },
      {
        path: 'memberships',
        component: MembershipsComponent,
      },
      {
        path: 'achievements',
        component: ProfileAchievementsComponent,
      },
      {
        path: 'learning-preferences',
        component: LearningPreferencesComponent,
      },
      {
        path: 'languages',
        component: LanguagesComponent,
      },
      {
        path: 'skills',
        component: SkillsAndInterestsComponent,
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'general-information',
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
export class ProfileRoutingModule {}
