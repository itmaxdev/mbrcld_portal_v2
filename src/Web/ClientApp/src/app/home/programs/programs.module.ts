import { NgModule } from '@angular/core'
import { TableModule } from 'primeng/table'
import { ToastModule } from 'primeng/toast'
import { StepsModule } from 'primeng/steps'
import { ButtonModule } from 'primeng/button'
import { RatingModule } from 'primeng/rating'
import { TabViewModule } from 'primeng/tabview'
import { GalleriaModule } from 'primeng/galleria'
import { SplitButtonModule } from 'primeng/splitbutton'
import { ProgressBarModule } from 'primeng/progressbar'
import { CommonModule } from '@angular/common'
import { SecureStorage } from 'src/app/core/api-authorization/oauth-storage'
import { FileUploadModule } from '@iplab/ngx-file-upload'
import { ConfirmDialogModule } from 'primeng/confirmdialog'
import { SharedModule } from 'src/app/shared/shared.module'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { ProgramsRoutingModule } from './programs-routing.module'
import { AddVideoComponent } from './common/add-video.component'
import { AddContentComponent } from './common/add-content.component'
import { AddMeetingComponent } from './common/add-meeting.component'
import { AddAttachmentComponent } from './common/add-attachment.component'
import { MessageChatComponent } from './message-chat/message-chat.component'
import { ProgramApplyComponent } from './program-apply/program-apply.component'
import { ProgramsListComponent } from './programs-list/programs-list.component'
import { ProgramsList3Component } from './programs-list3/programs-list3.component'
import { ProgramsAdminComponent } from './programs-admin/programs-admin.component'
import { ProgramsAlumniComponent } from './programs-alumni/programs-alumni.component'
import { ReferencesComponent } from './program-apply/references/references.component'
import { ProgramDetailsComponent } from './program-details/program-details.component'
import { ProgramsProjectEditComponent } from './common/programs-project-edit.component'
import { ProgramsModulesComponent } from './programs-modules/programs-modules.component'
import { ProgramsSectionComponent } from './programs-section/programs-section.component'
import { ProgramsCoursesComponent } from './programs-courses/programs-courses.component'
import { AchievementsComponent } from './program-apply/achievements/achievements.component'
import { ProgramsMainPageComponent } from './programs-main-page/programs-main-page.component'
import { ProgramsMaterialsComponent } from './programs-materials/programs-materials.component'
import { ProgramSectionInfoCardComponent } from './common/program-section-info-card.component'
import { ProgramsLeadershipComponent } from './programs-leadership/programs-leadership.component'
import { AcknowledgmentComponent } from './program-apply/acknowledgment/acknowledgment.component'
import { ProgramsProjectGroupEditComponent } from './common/programs-project-group-edit.component'
import { ProjectCohortCardComponent } from './programs-alumni/common/project-cohort-card.component'
import { ProgramsNewContentComponent } from './programs-new-content/programs-new-content.component'
import { ProgramCohortCardComponent } from './programs-alumni/common/program-cohort-card.component'
import { ProgramSectionContentCardComponent } from './common/program-section-content-card.component'
import { UploadVideoComponent } from './program-apply/upload-video/upload-video.component'
import { SmartAssessmentComponent } from './program-apply/smart-assessment/smart-assessment.component'
import { ProgramsSectionListComponent } from './programs-section-list/programs-section-list.component'
import { ProgramQuestionsComponent } from './program-apply/program-questions/program-questions.component'
import { ProgramSuggestedViewComponent } from './programs-alumni/common/program-suggested-view.component'
import { ProgramCohortViewComponent } from './programs-alumni/program-cohort-view/program-cohort-view.component'
import { ProgramsSubmittedProjectComponent } from './programs-submitted-project/programs-submitted-project.component'
import { ProgramsModulesApplicantComponent } from './programs-modules-applicant/programs-modules-applicant.component'
import { ProgramsModulesInstructorComponent } from './programs-modules-instructor/programs-modules-instructor.component'
import { ProgramsModulesAdminComponent } from './programs-modules-admin/programs-modules-admin.component'
import { ProgramsCohortModulesComponent } from './programs-alumni/programs-cohort-modules/programs-cohort-modules.component'
import { ProgramsCohortSectionListComponent } from './programs-alumni/programs-cohort-sections/programs-cohort-sections.component'
import { ProgramsCohortSectionComponent } from './programs-alumni/programs-cohort-section-contents/programs-cohort-section-contents.component'
import { ProgramActiveProgramsComponent } from './programs-registrant/programs-active-programs.component'
import { ProgramActiveProgramComponent } from './programs-registrant/programs-active-program.component'
import { ProgramsNewNewsFeedComponent } from './programs-new-newsfeed/programs-new-newsfeed.component'
import { AddTextNewsfeedComponent } from './common/add-newsfeed-text.component'
import { AddNewsFeedVideoComponent } from './common/add-newsfeed-video.component'
import { AddNewsfeedDocumentComponent } from './common/add-newsfeed-document.component'
import { AddNewsFeedMeetingComponent } from './common/add-newsfeed-meeting.component'
import { AddStickyNotesComponent } from './common/add-sticky-note.component'
import { ProgramNewsFeedContentCardComponent } from './common/program-newsfeed-content-card.component'
import { PreviousProgramComponent } from './programs-registrant/programs-previous-program.component'

@NgModule({
  declarations: [
    AddVideoComponent,
    AddContentComponent,
    AddContentComponent,
    ReferencesComponent,
    AddMeetingComponent,
    MessageChatComponent,
    ProgramApplyComponent,
    AchievementsComponent,
    ProgramsListComponent,
    AddAttachmentComponent,
    ProgramsList3Component,
    ProgramsAdminComponent,
    ProgramsAlumniComponent,
    AcknowledgmentComponent,
    AddStickyNotesComponent,
    ProgramDetailsComponent,
    UploadVideoComponent,
    SmartAssessmentComponent,
    ProgramsCoursesComponent,
    ProgramsModulesComponent,
    AddTextNewsfeedComponent,
    ProgramsSectionComponent,
    ProgramsMainPageComponent,
    AddNewsFeedVideoComponent,
    ProgramQuestionsComponent,
    ProgramCohortViewComponent,
    ProgramCohortCardComponent,
    ProgramsMaterialsComponent,
    ProjectCohortCardComponent,
    ProgramsLeadershipComponent,
    AddNewsFeedMeetingComponent,
    ProgramsNewContentComponent,
    AddNewsfeedDocumentComponent,
    ProgramsSectionListComponent,
    ProgramsProjectEditComponent,
    ProgramSuggestedViewComponent,
    ProgramSectionInfoCardComponent,
    ProgramsProjectGroupEditComponent,
    ProgramsSubmittedProjectComponent,
    ProgramsModulesApplicantComponent,
    ProgramSectionContentCardComponent,
    ProgramsModulesInstructorComponent,
    ProgramsModulesAdminComponent,
    ProgramsAlumniComponent,
    ProgramCohortViewComponent,
    ProgramsCohortModulesComponent,
    ProgramsCohortSectionListComponent,
    ProgramsCohortSectionComponent,
    ProgramActiveProgramsComponent,
    ProgramActiveProgramComponent,
    PreviousProgramComponent,
    ProgramsNewNewsFeedComponent,
    ProgramNewsFeedContentCardComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    StepsModule,
    TableModule,
    ToastModule,
    ButtonModule,
    SharedModule,
    RatingModule,
    TabViewModule,
    GalleriaModule,
    FileUploadModule,
    ProgressBarModule,
    ConfirmDialogModule,
    ReactiveFormsModule,
    ProgramsRoutingModule,
    SplitButtonModule,
  ],
  providers: [SecureStorage],
})
export class ProgramsModule {}
