import { NewsfeedComponent } from './components/newsfeed'
import { CardComponent } from './components/card.component'
import { LanguageSwitcherComponent } from './components/language-switcher.component'
import { NationalityDropdownComponent } from './components/nationality-dropdown.component'
import { PasswordValidatorComponent } from './components/password-validator.component'
import { PhoneNumberInputComponent } from './components/phone-number-input.component'
import { InstructorProfileComponent } from './components/instructor-profile.component'
import { ProfileSectionInfoCardComponent } from './components/profile-section-info-card.component'
import { ControlValidationErrorDirective } from './directives/control-validation-error.directive'
import { PasswordValidatorDirective } from './directives/password-validator.directive'
import { PrimeNGi18nDirective } from './directives/primeng-i18n.directive'
import { UseDateRangeDirective } from './directives/use-date-range.directive'
import { UseUtcDirective } from './directives/use-utc.directive'
import { ValidatePhoneNumberDirective } from './directives/validate-phone-number.directive'
import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { RouterModule } from '@angular/router'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { MenuModule } from 'primeng/menu'
import { CardModule } from 'primeng/card'
import { ToastModule } from 'primeng/toast'
import { RatingModule } from 'primeng/rating'
import { ButtonModule } from 'primeng/button'
import { DialogModule } from 'primeng/dialog'
import { EditorModule } from 'primeng/editor'
import { TooltipModule } from 'primeng/tooltip'
import { CalendarModule } from 'primeng/calendar'
import { DropdownModule } from 'primeng/dropdown'
import { CheckboxModule } from 'primeng/checkbox'
import { PasswordModule } from 'primeng/password'
import { InputMaskModule } from 'primeng/inputmask'
import { InputTextModule } from 'primeng/inputtext'
import { AccordionModule } from 'primeng/accordion'
import { SlideMenuModule } from 'primeng/slidemenu'
import { FileUploadModule } from 'primeng/fileupload'
import { InputNumberModule } from 'primeng/inputnumber'
import { ProgressBarModule } from 'primeng/progressbar'
import { SelectButtonModule } from 'primeng/selectbutton'
import { ToggleButtonModule } from 'primeng/togglebutton'
import { ConfirmDialogModule } from 'primeng/confirmdialog'
import { OverlayPanelModule } from 'primeng/overlaypanel'
import { InputTextareaModule } from 'primeng/inputtextarea'
import { ProgressSpinnerModule } from 'primeng/progressspinner'
import { LanguagePipe } from './pipes/language.pipe'
import { FullCalendarModule } from '@fullcalendar/angular'
import listPlugin from '@fullcalendar/list'
import timeGridPlugin from '@fullcalendar/timegrid'
import interactionPlugin from '@fullcalendar/interaction'
import { PostsCardComponent } from './components/posts-card.component'
import { SurveyCardComponent } from './components/survey-card.component'
import { MemberCardComponent } from './components/member-card.component'
import { ProgressBarComponent } from './components/progress-bar.component'
import { SocialPanelComponent } from './components/social-panel.component'
import { ImageUploadComponent } from './components/image-upload.component'
import { CoursesCardComponent } from './components/courses-card.component'
import { ProfileImageComponent } from './components/profile-image.component'
import { SectionsCardComponent } from './components/sections-card.component'
import { EnrolledCardComponent } from './components/enrolled-card.component'
import { ChatFileItemComponent } from './components/chat-file-item.component'
import { SuggestedCardComponent } from './components/suggested-card.component'
import { MaterialsCardComponent } from './components/materials-card.component'
import { ChatMyMessageComponent } from './components/chat-my-message.component'
import { CommentSectionComponent } from './components/comment-section.component'
import { ProgressSpinnerComponent } from './components/progress-spinner.component'
import { ClassMemberItemController } from './components/class-member-item.component'
import { LiveClassesItemController } from './components/live-classes-item.component'
import { ProfileInformationComponent } from './components/profile-information.component'
import { LeadershipProjectsComponent } from './components/leadership-projects.component'
import { ChatParticipantItemComponent } from './components/chat-participant-item.component'
import { ClassMemberItemSmallController } from './components/class-member-item-small.component'
import { ChatParticipantMessageComponent } from './components/chat-participant-message.component'
import { ViewApplicantPageComponent } from '../home/programs/view-applicant-page/view-applicant-page.component'
import { SectionDataService } from './services/section-data.service'
import { ModulesMaterialsCardComponent } from './components/modules-materials-card.component'
import { MeetingRoomsComponent } from './components/meeting-rooms.component'
import { DisclaimerComponent } from './components/disclaimer.component'
import { DashboardCardComponent } from './components/dashboard-card'
import { TextContentComponent } from '../home/programs/common/text-content.component'
import { VideoContentComponent } from '../home/programs/common/video-content.component'
import { DocumentContentComponent } from '../home/programs/common/document-content.component'
import { MeetingContentComponent } from '../home/programs/common/meeting-content.component'
import { NewsFeedSocialPanelComponent } from './components/newsfeed-social-panel.component'
import { SideBoxComponent } from './components/sidebox/sideBox.component'

FullCalendarModule.registerPlugins([interactionPlugin, timeGridPlugin, listPlugin])

const PrimeNgModules = [
  CardModule,
  ToastModule,
  ButtonModule,
  DialogModule,
  RatingModule,
  EditorModule,
  CalendarModule,
  CheckboxModule,
  DropdownModule,
  PasswordModule,
  InputMaskModule,
  SlideMenuModule,
  InputTextModule,
  FileUploadModule,
  InputNumberModule,
  ProgressBarModule,
  OverlayPanelModule,
  SelectButtonModule,
  ToggleButtonModule,
  FullCalendarModule,
  ConfirmDialogModule,
  InputTextareaModule,
  ProgressSpinnerModule,
]

const Components = [
  SideBoxComponent,
  CardComponent,
  VideoContentComponent,
  DocumentContentComponent,
  MeetingContentComponent,
  NewsfeedComponent,
  PostsCardComponent,
  SurveyCardComponent,
  MemberCardComponent,
  DisclaimerComponent,
  ProgressBarComponent,
  SocialPanelComponent,
  ImageUploadComponent,
  NewsFeedSocialPanelComponent,
  CoursesCardComponent,
  SectionsCardComponent,
  MeetingRoomsComponent,
  ProfileImageComponent,
  EnrolledCardComponent,
  ChatFileItemComponent,
  SuggestedCardComponent,
  ChatMyMessageComponent,
  DashboardCardComponent,
  MaterialsCardComponent,
  CommentSectionComponent,
  ProgressSpinnerComponent,
  LiveClassesItemController,
  PhoneNumberInputComponent,
  ClassMemberItemController,
  LanguageSwitcherComponent,
  InstructorProfileComponent,
  ViewApplicantPageComponent,
  PasswordValidatorComponent,
  ProfileInformationComponent,
  LeadershipProjectsComponent,
  NationalityDropdownComponent,
  ChatParticipantItemComponent,
  ClassMemberItemSmallController,
  ChatParticipantMessageComponent,
  ProfileSectionInfoCardComponent,
  ModulesMaterialsCardComponent,
  TextContentComponent,
]

const Directives = [
  UseUtcDirective,
  PrimeNGi18nDirective,
  UseDateRangeDirective,
  PasswordValidatorDirective,
  ValidatePhoneNumberDirective,
  ControlValidationErrorDirective,
]

const Pipes = [LanguagePipe]

@NgModule({
  declarations: [...Components, ...Directives, ...Pipes],
  imports: [
    MenuModule,
    CardModule,
    FormsModule,
    ToastModule,
    RatingModule,
    RouterModule,
    EditorModule,
    ButtonModule,
    CommonModule,
    DialogModule,
    TooltipModule,
    CalendarModule,
    DropdownModule,
    AccordionModule,
    InputTextModule,
    FileUploadModule,
    ProgressBarModule,
    ConfirmDialogModule,
    ReactiveFormsModule,
    ProgressSpinnerModule,
  ],
  exports: [
    MenuModule,
    FormsModule,
    CommonModule,
    RouterModule,
    TooltipModule,
    ReactiveFormsModule,
    ...Components,
    ...Directives,
    ...Pipes,
    ...PrimeNgModules,
  ],
  providers: [SectionDataService],
})
export class SharedModule {}
