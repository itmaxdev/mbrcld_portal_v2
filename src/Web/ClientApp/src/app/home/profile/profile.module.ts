import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { ProfileRoutingModule } from './profile-routing.module'
import { InputTextareaModule } from 'primeng/inputtextarea'
import { GeneralInformationComponent } from './general-information/general-information.component'
import { ContactDetailsComponent } from './contact-details/contact-details.component'
import { IdentityAndDocumentsComponent } from './identity-and-documents/identity-and-documents.component'
import { ProfessionalExperienceComponent } from './professional-experience/professional-experience.component'
import { EducationComponent } from './education/education.component'
import { TrainingCoursesComponent } from './training-courses/training-courses.component'
import { MembershipsComponent } from './memberships/memberships.component'
import { LanguagesComponent } from './languages/languages.component'
import { SkillsAndInterestsComponent } from './skills-and-interests/skills-and-interests.component'
import { LearningPreferencesComponent } from './learning-preferences/learning-preferences.component'
import { ProfileComponent } from './profile.component'
import { DocumentUploadComponent } from './identity-and-documents/document-upload.component'
import { HomeSharedModule } from '../shared/home-shared.module'
import { KeyFilterModule } from 'primeng/keyfilter'
import { AboutYourselfComponent } from './about-yourself/about-yourself.component'
import { AboutUniversityComponent } from './about-university/about-university.component'
import { GeneralInformationApplicantComponent } from './general-information-applicant/general-information-applicant.component'
import { ProfileAchievementsComponent } from './profile-achievements/profile-achievements.component'
import { TeamComponent } from './team/team.component'

@NgModule({
  declarations: [
    ProfileComponent,
    GeneralInformationComponent,
    ContactDetailsComponent,
    IdentityAndDocumentsComponent,
    ProfessionalExperienceComponent,
    EducationComponent,
    TrainingCoursesComponent,
    MembershipsComponent,
    LanguagesComponent,
    SkillsAndInterestsComponent,
    LearningPreferencesComponent,
    DocumentUploadComponent,
    AboutYourselfComponent,
    AboutUniversityComponent,
    GeneralInformationApplicantComponent,
    ProfileAchievementsComponent,
    TeamComponent,
  ],
  imports: [
    ProfileRoutingModule,
    SharedModule,
    HomeSharedModule,
    KeyFilterModule,
    InputTextareaModule,
  ],
  exports: [GeneralInformationComponent, TeamComponent],
})
export class ProfileModule {}
