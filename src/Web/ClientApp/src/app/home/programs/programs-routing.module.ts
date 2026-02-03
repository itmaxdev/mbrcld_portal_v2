import { AddStickyNotesComponent } from './common/add-sticky-note.component'
import { AchievementsComponent } from './program-apply/achievements/achievements.component'
import { AcknowledgmentComponent } from './program-apply/acknowledgment/acknowledgment.component'
import { ProgramApplyComponent } from './program-apply/program-apply.component'
import { ProgramQuestionsComponent } from './program-apply/program-questions/program-questions.component'
import { ReferencesComponent } from './program-apply/references/references.component'
import { SmartAssessmentComponent } from './program-apply/smart-assessment/smart-assessment.component'
import { UploadVideoComponent } from './program-apply/upload-video/upload-video.component'
import { ProgramDetailsComponent } from './program-details/program-details.component'
import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { ProgramsModulesComponent } from './programs-modules/programs-modules.component'
import { ProgramsMaterialsComponent } from './programs-materials/programs-materials.component'
import { AddContentComponent } from './common/add-content.component'
import { AddVideoComponent } from './common/add-video.component'
import { AddAttachmentComponent } from './common/add-attachment.component'
import { ProgramsSectionComponent } from './programs-section/programs-section.component'
import { ProgramsNewContentComponent } from './programs-new-content/programs-new-content.component'
import { ProgramsMainPageComponent } from './programs-main-page/programs-main-page.component'
import { ProgramsSectionListComponent } from './programs-section-list/programs-section-list.component'
import { AddMeetingComponent } from './common/add-meeting.component'
import { ProgramsProjectEditComponent } from './common/programs-project-edit.component'
import { ProgramsSubmittedProjectComponent } from './programs-submitted-project/programs-submitted-project.component'
import { MessageChatComponent } from './message-chat/message-chat.component'
import { ProgramCohortViewComponent } from './programs-alumni/program-cohort-view/program-cohort-view.component'
import { ProgramsProjectGroupEditComponent } from './common/programs-project-group-edit.component'
import { ViewApplicantPageComponent } from './view-applicant-page/view-applicant-page.component'
import { ProgramsCohortSectionListComponent } from './programs-alumni/programs-cohort-sections/programs-cohort-sections.component'
import { ProgramsCohortSectionComponent } from './programs-alumni/programs-cohort-section-contents/programs-cohort-section-contents.component'
import { ProgramsCohortModulesComponent } from './programs-alumni/programs-cohort-modules/programs-cohort-modules.component'
import { ProgramActiveProgramComponent } from './programs-registrant/programs-active-program.component'
import { ProgramsNewNewsFeedComponent } from './programs-new-newsfeed/programs-new-newsfeed.component'
import { AddTextNewsfeedComponent } from './common/add-newsfeed-text.component'
import { AddNewsFeedVideoComponent } from './common/add-newsfeed-video.component'
import { AddNewsfeedDocumentComponent } from './common/add-newsfeed-document.component'
import { AddNewsFeedMeetingComponent } from './common/add-newsfeed-meeting.component'
import { PreviousProgramComponent } from './programs-registrant/programs-previous-program.component'

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: ProgramsMainPageComponent,
  },
  {
    path: 'cohort/:cohortId',
    component: ProgramCohortViewComponent,
  },
  {
    path: 'groupped-project/:projectId',
    component: ProgramsProjectGroupEditComponent,
  },
  {
    path: 'view/:programId',
    component: ProgramsMaterialsComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId',
    component: ProgramsModulesComponent,
  },
  {
    pathMatch: 'full',
    path: 'view/:programId/modules/:modulesId/edit',
    component: ProgramsNewNewsFeedComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/edit/add-text',
    component: AddTextNewsfeedComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/edit/edit-text/:textId',
    component: AddTextNewsfeedComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/edit/add-video',
    component: AddNewsFeedVideoComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/edit/edit-video/:videoId',
    component: AddNewsFeedVideoComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/edit/add-document',
    component: AddNewsfeedDocumentComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/edit/edit-document/:documentId',
    component: AddNewsfeedDocumentComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/edit/add-meeting',
    component: AddNewsFeedMeetingComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/edit/edit-meeting/:meetingId',
    component: AddNewsFeedMeetingComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/edit/add-sticknote',
    component: AddStickyNotesComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/edit/edit-sticknote/:sticknoteId',
    component: AddStickyNotesComponent,
  },
  {
    path:
      'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit/add-sticknote',
    component: AddStickyNotesComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/view-applicant/:applicantId',
    component: ViewApplicantPageComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/chat/:roomId',
    component: MessageChatComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/:projectId',
    component: ProgramsSubmittedProjectComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/materials/:materialsId',
    component: ProgramsSectionListComponent,
  },
  {
    path: 'view/:programId/project/:projectId',
    component: ProgramsProjectEditComponent,
  },
  {
    path: 'cohort/:cohortId',
    component: ProgramCohortViewComponent,
  },
  {
    path: 'cohort/:cohortId/modules/:modulesId',
    component: ProgramsCohortModulesComponent,
  },
  {
    path: 'cohort/:cohortId/modules/:modulesId/materials/:materialsId',
    component: ProgramsCohortSectionListComponent,
  },
  {
    path: 'cohort/:cohortId/modules/:modulesId/materials/:materialsId/content/:contentId',
    component: ProgramsCohortSectionComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId',
    component: ProgramsSectionComponent,
  },
  {
    path: 'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit',
    component: ProgramsNewContentComponent,
  },
  {
    path:
      'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit/add-content',
    component: AddContentComponent,
  },
  {
    path:
      'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit/edit-content/:contentId',
    component: AddContentComponent,
  },
  {
    path:
      'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit/add-video',
    component: AddVideoComponent,
  },
  {
    path:
      'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit/edit-video/:videoId',
    component: AddVideoComponent,
  },
  {
    path:
      'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit/edit-sticknote/:sticknoteId',
    component: AddStickyNotesComponent,
  },
  {
    path:
      'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit/add-document',
    component: AddAttachmentComponent,
  },
  {
    path:
      'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit/edit-attachment/:documentId',
    component: AddAttachmentComponent,
  },
  {
    path:
      'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit/add-meeting',
    component: AddMeetingComponent,
  },
  {
    path:
      'view/:programId/modules/:modulesId/materials/:materialsId/content/:sectionId/edit/edit-meeting/:meetingId',
    component: AddMeetingComponent,
  },
  {
    path: 'program-details',
    component: ProgramDetailsComponent,
  },
  {
    path: 'program/:programId',
    component: ProgramActiveProgramComponent,
  },
  {
    path: 'program/previous/:programId',
    component: PreviousProgramComponent,
  },
  {
    path: 'apply/:activeProgramId',
    component: ProgramApplyComponent,
    children: [
      {
        path: 'achievements',
        component: AchievementsComponent,
      },
      {
        path: 'references',
        component: ReferencesComponent,
      },
      {
        path: 'questions',
        component: ProgramQuestionsComponent,
      },
      {
        path: 'upload-video',
        component: UploadVideoComponent,
      },
      {
        path: 'smart-assessment',
        component: SmartAssessmentComponent,
      },
      {
        path: 'acknowledgment',
        component: AcknowledgmentComponent,
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'achievements',
      },
    ],
  },
  {
    path: '**',
    redirectTo: '',
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class ProgramsRoutingModule {}
